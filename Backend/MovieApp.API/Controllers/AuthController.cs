// MovieApp.API/Controllers/AuthController.cs
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieApp.Business.Abstract;
using MovieApp.Business.Utilities.Results;
using MovieApp.DTO.DTOs.Auth;
using MovieApp.DTO.DTOs.AuthDtos;
using MovieApp.Entity.Entities;
using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace MovieApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;
        private readonly UrlEncoder _urlEncoder;
        private readonly IMapper _mapper;
        private readonly AppUrlsOptions _urls;
        private readonly ILogger<AuthController> _logger;
        private readonly IJwtService _jwtService;

        public AuthController(
            IAuthService authService,
            IEmailSender emailSender,
            UrlEncoder urlEncoder,
            IMapper mapper,
            UserManager<User> userManager,
            IOptions<AppUrlsOptions> urls,
            ILogger<AuthController> logger,
            IJwtService jwtService)
        {
            _authService = authService;
            _emailSender = emailSender;
            _urlEncoder = urlEncoder;
            _mapper = mapper;
            _userManager = userManager;
            _urls = urls.Value;
            _logger = logger;
            _jwtService = jwtService;
        }

        /
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reg = await _authService.RegisterAsync(dto);
            if (!reg.Success)
                return BadRequest(reg.Message);

            try
            {
                var (ok, err, raw) = await _authService.GenerateEmailConfirmationTokenAsync(dto.Email);
                if (!ok) throw new Exception(err);

                var user = await _authService.FindUserByEmailAsync(dto.Email);
                var enc = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(raw));
                var link = $"{_urls.Frontend}/confirm-email?userId={user.Id}&token={enc}";

                await _emailSender.SendEmailAsync(
                    dto.Email,
                    "MovieApp – E-posta Doğrulama",
                    $@"Merhaba {user.UserName},<br/><br/>
                       Hesabınızı doğrulamak için <a href=""{link}"">tıklayın</a>.");

                return Ok(new { message = "Kayıt başarılı. Lütfen e-posta adresinizi doğrulayın." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Doğrulama e-postası gönderilemedi");
                return Ok(new
                {
                    message = "Kayıt başarılı fakat doğrulama e-postası gönderilemedi. Lütfen sonra tekrar deneyin."
                });
            }
        }

        
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(token))
                return BadRequest("Eksik userId veya token.");

            string decoded;
            try
            {
                decoded = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            }
            catch
            {
                return BadRequest("Geçersiz token (decode).");
            }

            var (succ, err) = await _authService.ConfirmEmailAsync(userId.ToString(), decoded);
            if (!succ)
                return BadRequest($"E-posta doğrulama başarısız: {err}");

            var user = await _userManager.FindByIdAsync(userId.ToString());
            var tokenData = await _jwtService.CreateTokenAsync(user);

            var cookieOpts = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = tokenData.ExpiresAt
            };
            Response.Cookies.Append("refreshToken", tokenData.RefreshToken, cookieOpts);

            return Ok(new
            {
                accessToken = tokenData.AccessToken,
                message = "E-posta doğrulandı, giriş yapıldı."
            });
        }

  

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var loginResult = await _authService.LoginAsync(dto);
            if (!loginResult.Success) return BadRequest(loginResult.Message);

            var cookieOpts = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", loginResult.Data.RefreshToken, cookieOpts);

            return Ok(new { accessToken = loginResult.Data.AccessToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var token = Request.Cookies["refreshToken"];
            if (!string.IsNullOrEmpty(token))
                await _authService.RevokeRefreshTokenAsync(token);

            Response.Cookies.Delete("refreshToken");
            return NoContent();
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var token = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest("Refresh token bulunamadı.");

            var res = await _authService.RefreshTokenAsync(token);
            if (!res.Success) return BadRequest(res.Message);

            var cookieOpts = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", res.Data.RefreshToken, cookieOpts);

            return Ok(new { accessToken = res.Data.AccessToken });
        }

        

        [HttpPost("resend-confirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmation([FromBody] ForgotConfirmationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _authService.FindUserByEmailAsync(dto.Email);
            if (user == null) return Ok("Eğer kayıtlı bir hesabınız varsa, onay maili gönderildi.");

            if (await _userManager.IsEmailConfirmedAsync(user))
                return Ok("E-posta zaten onaylı.");

            var (ok, err, raw) = await _authService.GenerateEmailConfirmationTokenAsync(dto.Email);
            if (!ok) return StatusCode(500, $"Token üretilemedi: {err}");

            var enc = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(raw));
            var link = $"{_urls.Frontend}/confirm-email?userId={user.Id}&token={enc}";
            var body = $@"Merhaba {user.UserName},<br/><br/>
                           Hesabınızı onaylamak için <a href=""{link}"">tıklayın</a>.";

            await _emailSender.SendEmailAsync(dto.Email, "MovieApp – E-posta Onayı", body);
            return Ok("Onay maili tekrar gönderildi. Lütfen e-postanızı kontrol edin.");
        }

     

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var (ok, err, rawToken) =
                await _authService.GeneratePasswordResetTokenAsync(dto.Email);

            if (!ok) return BadRequest(err);

            var user = await _authService.FindUserByEmailAsync(dto.Email);
            var enc = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));
            var link = $"{_urls.Frontend}/reset-password?userId={user.Id}&token={enc}";

            var body = $@"Merhaba {user.UserName},<br/><br/>
                          Şifrenizi sıfırlamak için aşağıdaki linke tıklayın:<br/>
                          <a href=""{link}"">Şifreyi Sıfırla</a><br/><br/>
                          Eğer bu isteği siz yapmadıysanız, bu e-postayı yok sayabilirsiniz.";

            await _emailSender.SendEmailAsync(dto.Email, "MovieApp – Şifre Sıfırlama", body);
            return Ok(new { message = "Şifre sıfırlama bağlantısı gönderildi." });
        }

   

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            string decodedToken;
            try
            {
                decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(dto.Token));
            }
            catch
            {
                return BadRequest("Token çözümlenemedi.");
            }

            var (ok, err) = await _authService.ResetPasswordAsync(
                                dto.UserId.ToString(),
                                decodedToken,
                                dto.NewPassword);

            return ok
                ? Ok(new { message = "Şifreniz başarıyla güncellendi." })
                : BadRequest(err);
        }

      

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var res = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!res.Succeeded)
            {
                var errors = string.Join(" | ", res.Errors.Select(e => e.Description));
                return BadRequest(errors);
            }

            return Ok(new { message = "Şifreniz başarıyla değiştirildi." });
        }
    }
}
