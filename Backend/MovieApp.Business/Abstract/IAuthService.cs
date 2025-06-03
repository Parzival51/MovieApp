using MovieApp.Business.Utilities.Results;
using MovieApp.DTO.DTOs.Auth;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IAuthService
    {
        // Mevcut metotlar
        Task<IDataResult<TokenResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<IDataResult<TokenResponseDto>> LoginAsync(LoginDto loginDto);
        Task<IDataResult<TokenResponseDto>> RefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);


        Task<User> FindUserByEmailAsync(string email);

        Task<(bool Succeeded, string ErrorMessage, string Token)> GenerateEmailConfirmationTokenAsync(string email);

        Task<(bool Succeeded, string ErrorMessage)> ConfirmEmailAsync(string userId, string token);

        Task<(bool Succeeded, string ErrorMessage, string Token)> GeneratePasswordResetTokenAsync(string email);

        Task<(bool Succeeded, string ErrorMessage)> ResetPasswordAsync(string userId, string token, string newPassword);
    }
}
