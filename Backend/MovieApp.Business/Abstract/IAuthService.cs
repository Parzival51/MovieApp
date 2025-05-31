using MovieApp.Business.Utilities.Results;
using MovieApp.DTO.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<TokenResponseDto>> RegisterAsync(RegisterDto registerDto);
        Task<IDataResult<TokenResponseDto>> LoginAsync(LoginDto loginDto);
        Task<IDataResult<TokenResponseDto>> RefreshTokenAsync(string refreshToken);

        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
