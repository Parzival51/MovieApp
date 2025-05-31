using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Business.Abstract;
using MovieApp.Business.Configurations;
using MovieApp.DataAccess.Abstract;
using MovieApp.DTO.DTOs.Auth;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class JwtService : IJwtService
    {
        private readonly JwtTokenOptions _opts;
        private readonly UserManager<User> _userMgr;
        private readonly IRefreshTokenRepository _refreshRepo;

        public JwtService(
            IOptions<JwtTokenOptions> opts,
            UserManager<User> userMgr,
            IRefreshTokenRepository refreshRepo)
        {
            _opts = opts.Value;
            _userMgr = userMgr;
            _refreshRepo = refreshRepo;
        }

        public async Task<TokenResponseDto> CreateTokenAsync(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,           user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userMgr.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opts.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _opts.Issuer,
                audience: _opts.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_opts.AccessTokenExpiration),
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            var refresh = new RefreshToken
            {
                Token = GenerateSecureToken(),
                ExpiresAt = DateTime.UtcNow.AddMinutes(_opts.RefreshTokenExpiration),
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id
            };
            await _refreshRepo.AddAsync(refresh);
            await _refreshRepo.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refresh.Token,
                ExpiresAt = jwt.ValidTo
            };
        }

        private static string GenerateSecureToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
