// MovieApp.Business/Concrete/AuthManager.cs
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using MovieApp.Business.Abstract;
using MovieApp.Business.Utilities.Results;
using MovieApp.DataAccess.Abstract;
using MovieApp.DTO.DTOs.Auth;
using MovieApp.Entity.Entities;
using System;
using System.Linq;
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
        private readonly ILogger<AuthManager> _logger;

        public AuthManager(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IJwtService jwtService,
            IRefreshTokenRepository refreshRepo,
            IMapper mapper,
            ILogger<AuthManager> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _refreshRepo = refreshRepo;
            _mapper = mapper;
            _logger = logger;
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
                _logger.LogWarning("Kayıt başarısız: {Errors}", combined);
                return new ErrorDataResult<TokenResponseDto>(combined);
            }

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _jwtService.CreateTokenAsync(user);
            _logger.LogInformation("Yeni kullanıcı kayıt oldu: {Email}", user.Email);
            return new SuccessDataResult<TokenResponseDto>(token, "Kayıt ve rol atama başarılı");
        }

        public async Task<IDataResult<TokenResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                _logger.LogWarning("Giriş denenirken kullanıcı bulunamadı: {Email}", loginDto.Email);
                return new ErrorDataResult<TokenResponseDto>("Kullanıcı bulunamadı.");
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning("Giriş denenirken e-posta onayı eksik: {Email}", loginDto.Email);
                return new ErrorDataResult<TokenResponseDto>("Lütfen önce e-posta adresinizi doğrulayın.");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
            if (!signInResult.Succeeded)
            {
                _logger.LogWarning("Geçersiz parola denemesi: {Email}", loginDto.Email);
                return new ErrorDataResult<TokenResponseDto>("Geçersiz e-posta veya şifre.");
            }

            var token = await _jwtService.CreateTokenAsync(user);
            _logger.LogInformation("Kullanıcı başarıyla giriş yaptı: {Email}", loginDto.Email);
            return new SuccessDataResult<TokenResponseDto>(token);
        }

        public async Task<IDataResult<TokenResponseDto>> RefreshTokenAsync(string refreshToken)
        {
            var existing = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (existing == null || existing.ExpiresAt < DateTime.UtcNow)
            {
                _logger.LogWarning("Geçersiz veya süresi geçmiş refresh token: {Token}", refreshToken);
                return new ErrorDataResult<TokenResponseDto>("Invalid or expired refresh token");
            }

            var user = await _userManager.FindByIdAsync(existing.UserId.ToString());
            if (user == null)
            {
                _logger.LogWarning("Refresh token sahibi kullanıcı bulunamadı: {UserId}", existing.UserId);
                return new ErrorDataResult<TokenResponseDto>("Kullanıcı bulunamadı.");
            }

            var newToken = await _jwtService.CreateTokenAsync(user);
            _logger.LogInformation("Refresh token başarıyla yenilendi: {UserId}", existing.UserId);
            return new SuccessDataResult<TokenResponseDto>(newToken);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var existing = await _refreshRepo.GetByTokenAsync(refreshToken);
            if (existing == null)
            {
                _logger.LogWarning("Revoke denendi ancak geçersiz token: {Token}", refreshToken);
                return;
            }

            existing.RevokedAt = DateTime.UtcNow;
            await _refreshRepo.SaveChangesAsync();
            _logger.LogInformation("Refresh token iptal edildi: {UserId}", existing.UserId);
        }

        public async Task<User> FindUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<(bool Succeeded, string ErrorMessage, string Token)> GenerateEmailConfirmationTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("Email confirmation token istenen kullanıcı bulunamadı: {Email}", email);
                return (false, "E-posta adresine sahip kullanıcı bulunamadı.", null);
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            _logger.LogInformation("Email confirmation token üretildi: {Email}", email);
            return (true, null, token);
        }

        public async Task<(bool Succeeded, string ErrorMessage)> ConfirmEmailAsync(string userId, string token)
        {
            if (!Guid.TryParse(userId, out var guid))
            {
                _logger.LogWarning("ConfirmEmailAsync: Geçersiz userId: {UserId}", userId);
                return (false, "Geçersiz userId.");
            }

            var user = await _userManager.FindByIdAsync(guid.ToString());
            if (user == null)
            {
                _logger.LogWarning("ConfirmEmailAsync: Kullanıcı bulunamadı: {UserId}", userId);
                return (false, "Kullanıcı bulunamadı.");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                _logger.LogInformation("E-posta doğrulandı: {Email}", user.Email);
                return (true, null);
            }

            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            _logger.LogWarning(
                "ConfirmEmailAsync başarısız. Email: {Email}, Token: {Token}, Hatalar: {Errors}",
                user.Email, token, errors);

            return (false, errors);
        }

        public async Task<(bool Succeeded, string ErrorMessage, string Token)> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning(
                    "Şifre sıfırlama token’ı istenen kullanıcı bulunamadı: {Email}", email);
                return (false, "E-posta adresine sahip kullanıcı bulunamadı.", null);
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                _logger.LogWarning(
                    "Şifre sıfırlama isteği ancak e-posta doğrulanmamış: {Email}", email);
                return (false, "E-posta doğrulanmamış kullanıcı.", null);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            _logger.LogInformation("Şifre sıfırlama token üretildi: {Email}", email);
            return (true, null, token);
        }

        public async Task<(bool Succeeded, string ErrorMessage)> ResetPasswordAsync(string userId, string token, string newPassword)
        {
            if (!Guid.TryParse(userId, out var guid))
            {
                _logger.LogWarning("ResetPasswordAsync: Geçersiz userId: {UserId}", userId);
                return (false, "Geçersiz userId.");
            }

            var user = await _userManager.FindByIdAsync(guid.ToString());
            if (user == null)
            {
                _logger.LogWarning("ResetPasswordAsync: Kullanıcı bulunamadı: {UserId}", userId);
                return (false, "Kullanıcı bulunamadı.");
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                _logger.LogInformation("Şifre başarıyla sıfırlandı: {Email}", user.Email);
                return (true, null);
            }

            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            _logger.LogWarning(
                "ResetPasswordAsync başarısız. Email: {Email}, Token: {Token}, Hatalar: {Errors}",
                user.Email, token, errors);

            return (false, errors);
        }
    }
}
