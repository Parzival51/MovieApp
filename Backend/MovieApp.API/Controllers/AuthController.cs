using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.Auth;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        private void SetRefreshCookie(string token, int days = 7)
        {
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,                
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(days)
            };
            Response.Cookies.Append("refreshToken", token, options);
        }

        private void DeleteRefreshCookie()
        {
            Response.Cookies.Delete("refreshToken");
        }

        [HttpPost("register"), AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.Success) return BadRequest(result.Message);

            SetRefreshCookie(result.Data.RefreshToken);
            return Ok(new { accessToken = result.Data.AccessToken });
        }

        [HttpPost("login"), AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.Success) return Unauthorized(result.Message);

            SetRefreshCookie(result.Data.RefreshToken);
            return Ok(new { accessToken = result.Data.AccessToken });
        }

        
        [HttpPost("refresh-token"), AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest("Refresh token bulunamadı.");

            var result = await _authService.RefreshTokenAsync(token);
            if (!result.Success) return BadRequest(result.Message);

            SetRefreshCookie(result.Data.RefreshToken);
            return Ok(new { accessToken = result.Data.AccessToken });
        }

   
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(token))
                await _authService.RevokeRefreshTokenAsync(token);

            DeleteRefreshCookie();
            return NoContent();
        }
    }
}
