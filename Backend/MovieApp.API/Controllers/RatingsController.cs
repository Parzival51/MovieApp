using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.RatingDtos;
using MovieApp.Entity.Entities;
using System.Security.Claims;

namespace MovieApp.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratings;
        private readonly IMovieService _movies;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _ctx;       // ★ eklendi

        public RatingsController(
            IRatingService ratings,
            IMovieService movies,
            IMapper mapper,
            IHttpContextAccessor ctx)                     // ★ ctor param
        {
            _ratings = ratings;
            _movies = movies;
            _mapper = mapper;
            _ctx = ctx;
        }

        /*────────────────────────────  GET (filtreli)  ───────────────────────────
           /api/ratings?movieId={m}&mine=1   →  giriş yapan kullanıcının puanı
           /api/ratings?movieId={m}          →  filme ait tüm puanlar
           /api/ratings?userId  ={u}         →  kullanıcının tüm puanları
           (hiçbiri yoksa)                   →  tüm kayıtlar
        -------------------------------------------------------------------------*/
        [HttpGet, AllowAnonymous]
        public async Task<ActionResult<IEnumerable<RatingListDto>>> Get(
            [FromQuery] Guid? movieId,
            [FromQuery] Guid? userId,
            [FromQuery] bool mine = false)
        {
            var list = await _ratings.GetAllAsync();

            if (movieId is not null)
                list = list.Where(r => r.MovieId == movieId);

            if (userId is not null)
                list = list.Where(r => r.UserId == userId);

            if (mine)
            {
                var uidStr = _ctx.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (uidStr is null) return Unauthorized();

                var uid = Guid.Parse(uidStr);
                list = list.Where(r => r.UserId == uid);
            }

            return Ok(_mapper.Map<IEnumerable<RatingListDto>>(list));
        }

        /*────────────────────────────  GET by Id  ───────────────────────────────*/
        [HttpGet("{id:guid}"), AllowAnonymous]
        public async Task<ActionResult<RatingDetailDto>> GetById(Guid id)
        {
            var rating = await _ratings.GetByIdAsync(id);
            return rating is null
                 ? NotFound()
                 : Ok(_mapper.Map<RatingDetailDto>(rating));
        }

        /*────────────────────────────  UPSERT  ───────────────────────────────────*/
        /// <summary> Aynı kullanıcı-film kaydı varsa günceller, yoksa ekler. </summary>
        [HttpPost("upsert"), Authorize]                     // ★ route sabitlendi
        public async Task<ActionResult<RatingDetailDto>> Upsert([FromBody] CreateRatingDto dto)
        {
            if (await _movies.GetByIdAsync(dto.MovieId) is null)
                return NotFound($"Movie {dto.MovieId} not found");

            var entity = _mapper.Map<Rating>(dto);        // UserId → GenericManager
            var saved = await _ratings.AddOrUpdateAsync(entity);
            var payload = _mapper.Map<RatingDetailDto>(saved);

            return Ok(payload);                            // 200 + güncel satır
        }

        /*────────────────────────────  DELETE  ───────────────────────────────────*/
        [HttpDelete("{id:guid}"), Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rating = await _ratings.GetByIdAsync(id);
            if (rating is null) return NotFound();

            await _ratings.DeleteAsync(rating);
            return NoContent();
        }
    }
}
