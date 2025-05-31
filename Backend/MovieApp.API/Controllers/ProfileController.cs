using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.UserDtos;
using MovieApp.Entity.Entities;
using System.Security.Claims;

namespace MovieApp.API.Controllers
{
    [Authorize]                                 // ↔ sadece oturum sahipleri
    [Route("api/profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userSvc;
        private readonly IFavoriteService _favSvc;
        private readonly IWatchlistService _watchSvc;
        private readonly IMapper _mapper;

        public ProfileController(
            IUserService userSvc,
            IMapper mapper)
        {
            _userSvc = userSvc;
            _mapper = mapper;
        }

        [HttpGet]                           // GET /api/profile
        public async Task<IActionResult> GetMe()
        {
            var uid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = (await _userSvc.GetAllUsersAsync()).First(u => u.Id == uid);
            var favorites = await _favSvc.GetFilteredListAsync(f => f.UserId == uid);
            var watches = await _watchSvc.GetFilteredListAsync(w => w.UserId == uid);

            var dto = _mapper.Map<UserProfileDto>(user);
            dto.FavoritesCount = favorites.Count();
            dto.WatchlistCount = watches.Count();

            return Ok(dto);
        }
    }
}
