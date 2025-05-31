using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MovieApp.Business.Abstract;
using MovieApp.Business.Utilities.Results;
using MovieApp.DataAccess.Abstract;
using MovieApp.DTO.DTOs.Auth;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IRefreshTokenRepository _refreshRepo;
        private readonly IMapper _mapper;

        public AuthManager(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService,
            IRefreshTokenRepository refreshRepo,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _refreshRepo = refreshRepo;
            _mapper = mapper;
        }

        public async Task<IDataResult<TokenResponseDto>> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email
            };

            var createResult = await _userManager.CreateAsync(user, registerDto.Password);

            if (!createResult.Succeeded)
            {
                var messages = createResult.Errors
                                           .Select(e => e.Description)
                                           .Where(d => !string.IsNullOrEmpty(d));
                var combined = string.Join(" | ", messages);
                return new ErrorDataResult<TokenResponseDto>(combined);
            }

            await _userManager.AddToRoleAsync(user, "User");
            var token = await _jwtService.CreateTokenAsync(user);
            return new SuccessDataResult<TokenResponseDto>(token, "Kayıt ve rol atama başarılı");
        }

        public async Task<IDataResult<TokenResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return new ErrorDataResult<TokenResponseDto>("User not found");

            var signInResult = await _signInManager.PasswordSignInAsync(
                user, loginDto.Password, false, false);
            if (!signInResult.Succeeded)
                return new ErrorDataResult<TokenResponseDto>("Invalid credentials");

            var token = await _jwtService.CreateTokenAsync(user);
            return new SuccessDataResult<TokenResponseDto>(token);
        }

        public async Task<IDataResult<TokenResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            var existing = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (existing == null || existing.ExpiresAt < DateTime.UtcNow)
                return new ErrorDataResult<TokenResponseDto>("Invalid or expired refresh token");

            var user = await _userManager.FindByIdAsync(existing.UserId.ToString());
            var newToken = await _jwtService.CreateTokenAsync(user);
            return new SuccessDataResult<TokenResponseDto>(newToken);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var existing = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (existing == null) return;                   

            existing.RevokedAt = DateTime.UtcNow;
            await _refreshRepo.SaveChangesAsync();
        }
    }
}
