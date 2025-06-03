// MovieApp.API/Controllers/AuthController.cs
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MovieApp.Business.Abstract;
using MovieApp.Business.Utilities.Results;
using MovieApp.DTO.DTOs.Auth;
using MovieApp.DTO.DTOs.AuthDtos;
using MovieApp.Entity.Entities;
using System;
using System.Linq;
using System.Net;
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

        public AuthController(
            IAuthService authService,
            IEmailSender emailSender,
            UrlEncoder urlEncoder,
            IMapper mapper,
            UserManager<User> userManager,
            IOptions<AppUrlsOptions> urls)      
        {
            _authService = authService;
            _emailSender = emailSender;
            _urlEncoder = urlEncoder;
            _mapper = mapper;
            _userManager = userManager;
            _urls = urls.Value;          
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reg = await _authService.RegisterAsync(dto);
            if (!reg.Success) return BadRequest(reg.Message);

            var (ok, err, raw) = await _authService.GenerateEmailConfirmationTokenAsync(dto.Email);
            if (!ok) return StatusCode(500, $"E-posta token üretilemedi: {err}");

            var user = await _authService.FindUserByEmailAsync(dto.Email);
            var enc = WebUtility.UrlEncode(raw);
            var link = $"{_urls.Frontend}/confirm-email?userId={user.Id}&token={enc}";   // << yönlendirme

            await _emailSender.SendEmailAsync(dto.Email,
                "MovieApp – E-posta Doğrulama",
                $@"Merhaba {user.UserName},<br/><br/>
               Hesabınızı doğrulamak için <a href=""{link}"">tıklayın</a>.");

            return Ok(new { Message = "Kayıt başarılı. Lütfen e-posta adresinizi doğrulayın." });
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string token)
        {
            if (userId == Guid.Empty || string.IsNullOrWhiteSpace(token))
                return BadRequest("Eksik userId veya token.");

            var (succ, err) = await _authService.ConfirmEmailAsync(userId.ToString(),
                                                                   WebUtility.UrlDecode(token));
            return succ ? Ok("E-posta adresiniz doğrulandı.") :
                          BadRequest($"E-posta doğrulama başarısız: {err}");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var loginResult = await _authService.LoginAsync(dto);
            if (!loginResult.Success)
                return BadRequest(loginResult.Message);

            // Refresh token’ı bir HttpOnly cookie’de saklayacağız
            var refreshToken = loginResult.Data.RefreshToken;
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

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

            var result = await _authService.RefreshTokenAsync(token);
            if (!result.Success)
                return BadRequest(result.Message);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", result.Data.RefreshToken, cookieOptions);

            return Ok(new { accessToken = result.Data.AccessToken });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Ok("Şifre sıfırlama linki e-posta adresinize gönderildi.");

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("Lütfen önce e-posta adresinizi doğrulayın.");

            var raw = await _userManager.GeneratePasswordResetTokenAsync(user);
            var enc = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(raw));
            var link = $"{_urls.Frontend}/reset-password?userId={user.Id}&token={enc}"; // << yönlendirme

            await _emailSender.SendEmailAsync(user.Email,
                "MovieApp Şifre Sıfırlama",
                $@"Şifrenizi sıfırlamak için <a href=""{link}"">tıklayın</a>.");

            return Ok("Şifre sıfırlama linki e-posta adresinize gönderildi.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());
            if (user == null)
                return BadRequest("Geçersiz kullanıcı.");

            byte[] decodedBytes;
            try
            {
                decodedBytes = WebEncoders.Base64UrlDecode(model.Token);
            }
            catch
            {
                return BadRequest("Geçersiz veya bozuk token.");
            }

            var decodedToken = Encoding.UTF8.GetString(decodedBytes);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.NewPassword);
            if (!result.Succeeded)
            {
                var firstError = result.Errors.FirstOrDefault()?.Description
                                 ?? "Şifre sıfırlama sırasında hata oluştu.";
                return BadRequest(firstError);
            }

            return Ok("Şifreniz başarıyla sıfırlandı.");
        }

        [HttpPost("resend-confirmation")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmation([FromBody] ForgotConfirmationDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userObj = await _authService.FindUserByEmailAsync(dto.Email);
            if (userObj == null)
                return Ok("Eğer kayıtlı bir hesabınız varsa, onay maili gönderildi.");

            if (await _userManager.IsEmailConfirmedAsync(userObj))
                return Ok("E-posta zaten onaylı.");

            var (tokenSuccess, tokenError, rawEmailToken) =
                await _authService.GenerateEmailConfirmationTokenAsync(dto.Email);
            if (!tokenSuccess)
                return StatusCode(StatusCodes.Status500InternalServerError, $"Token üretilemedi: {tokenError}");

            var encodedToken = WebUtility.UrlEncode(rawEmailToken);
            var confirmLink = Url.Action(
                nameof(ConfirmEmail),
                "Auth",
                new { userId = userObj.Id, token = encodedToken },
                Request.Scheme);

            var subject = "MovieApp – E-posta Onay Maili (Tekrar)";
            var body = $@"
                Merhaba {userObj.UserName},<br/><br/>
                Hesabınızı onaylamak için aşağıdaki linke tıklayın:<br/>
                <a href=""{confirmLink}"">E-posta Adresinizi Doğrulayın</a><br/><br/>
                Eğer bu istekte bulunmadıysanız, bu e-postayı görmezden gelebilirsiniz.<br/><br/>
                MovieApp Ekibi
            ";
            await _emailSender.SendEmailAsync(dto.Email, subject, body);

            return Ok("Onay maili tekrar gönderildi. Lütfen e-posta adresinizi kontrol edin.");
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            if (userId == null)
                return Unauthorized("Kullanıcı bilgisi bulunamadı.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            var result = await _userManager.ChangePasswordAsync(
                user,
                dto.CurrentPassword,
                dto.NewPassword);

            if (!result.Succeeded)
            {
                foreach (var err in result.Errors)
                    ModelState.AddModelError(err.Code, err.Description);
                return BadRequest(ModelState);
            }

            return Ok("Şifreniz başarıyla değiştirildi.");
        }
    }
}
