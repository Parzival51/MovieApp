using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Context;
using MovieApp.DTO.DTOs.WatchListDtos;
using MovieApp.Entity.Entities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WatchlistsController : ControllerBase
    {
        private readonly MovieAppDbContext _ctx;          
        private readonly IGenericService<Watchlist> _svc;
        private readonly IMovieService _movieSvc;
        private readonly IMapper _mapper;

        public WatchlistsController(
            MovieAppDbContext ctx,         
            IGenericService<Watchlist> svc,
            IMovieService movieSvc,
            IMapper mapper)
        {
            _ctx = ctx;
            _svc = svc;
            _movieSvc = movieSvc;
            _mapper = mapper;
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> GetMine()
        {
            var uid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var list = await _ctx.Watchlists           
                                 .Include(w => w.Movie)
                                 .Where(w => w.UserId == uid)
                                 .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<WatchlistListDto>>(list));
        }

        [HttpGet("all"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var all = await _ctx.Watchlists
                                .Include(w => w.Movie)
                                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<WatchlistListDto>>(all));
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Add([FromBody] CreateWatchlistDto dto)
        {
            if (await _movieSvc.GetByIdAsync(dto.MovieId) is null)
                return NotFound("Movie not found.");

            var entity = _mapper.Map<Watchlist>(dto);
            var created = await _svc.CreateAsync(entity);

            created = await _ctx.Watchlists
                                .Include(w => w.Movie)
                                .FirstAsync(w => w.Id == created.Id);

            return Ok(_mapper.Map<WatchlistDetailDto>(created));
        }

        [HttpDelete("{id:guid}"), Authorize]
        public async Task<IActionResult> Remove(Guid id)
        {
            var w = await _svc.GetByIdAsync(id);
            if (w is null) return NotFound();

            var uid = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (w.UserId != uid && !User.IsInRole("Admin"))
                return Forbid();

            await _svc.DeleteAsync(w);
            return NoContent();
        }
    }
}
