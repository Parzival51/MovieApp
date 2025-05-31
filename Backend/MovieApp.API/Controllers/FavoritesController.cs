using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Context;
using MovieApp.DTO.DTOs.FavoriteDtos;
using MovieApp.Entity.Entities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace MovieApp.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : ControllerBase
    {
        private readonly MovieAppDbContext _ctx;         // ← yeni
        private readonly IGenericService<Favorite> _svc;
        private readonly IMovieService _movieSvc;
        private readonly IMapper _mapper;

        public FavoritesController(
            MovieAppDbContext ctx,           // ← ctor’a eklendi
            IGenericService<Favorite> svc,
            IMovieService movieSvc,
            IMapper mapper)
        {
            _ctx = ctx;
            _svc = svc;
            _movieSvc = movieSvc;
            _mapper = mapper;
        }

        /* ─── Login kullanıcısının favorileri ─── */
        [HttpGet, Authorize]
        public async Task<IActionResult> GetMine()
        {
            var uid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var list = await _ctx.Favorites
                                 .Include(f => f.Movie)
                                 .Where(f => f.UserId == uid)
                                 .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<FavoriteListDto>>(list));
        }

        /* (Admin gözetimi) */
        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var all = await _ctx.Favorites
                                .Include(f => f.Movie)
                                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<FavoriteListDto>>(all));
        }

        /* ─── Ekle ─── */
        [HttpPost, Authorize]
        public async Task<IActionResult> Add([FromBody] CreateFavoriteDto dto)
        {
            if (await _movieSvc.GetByIdAsync(dto.MovieId) is null)
                return NotFound("Movie not found.");

            var entity = _mapper.Map<Favorite>(dto);
            var created = await _svc.CreateAsync(entity);

            created = await _ctx.Favorites
                                .Include(f => f.Movie)
                                .FirstAsync(f => f.Id == created.Id);

            return Ok(_mapper.Map<FavoriteDetailDto>(created));
        }

        /* ─── Sil ─── */
        [HttpDelete("{id:guid}"), Authorize]
        public async Task<IActionResult> Remove(Guid id)
        {
            var fav = await _svc.GetByIdAsync(id);
            if (fav is null) return NotFound();

            var uid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (fav.UserId != uid && !User.IsInRole("Admin"))
                return Forbid();

            await _svc.DeleteAsync(fav);
            return NoContent();
        }
    }
}
