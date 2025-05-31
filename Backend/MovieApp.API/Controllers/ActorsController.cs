using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.ActorDtos;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using System.Security.Claims;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;
        private readonly IMapper _mapper;
        private readonly IActorRatingService _actorRatingService;

        public ActorsController(IActorService actorService, IMapper mapper ,IActorRatingService actorRatingService)
        {
            _actorService = actorService;
            _actorRatingService = actorRatingService;
            _mapper = mapper;
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var list = await _actorService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ActorListDto>>(list));
        }

        [HttpGet("{id:guid}"), AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            return actor is null
                ? NotFound()
                : Ok(_mapper.Map<ActorDetailDto>(actor));
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateActorDto dto)
        {
            var entity = _mapper.Map<Actor>(dto);
            var created = await _actorService.CreateAsync(entity);

            return CreatedAtAction(nameof(GetById),
                new { id = created.Id },
                _mapper.Map<ActorDetailDto>(created));
        }

        [HttpPut("{id:guid}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateActorDto dto)
        {
            if (id != dto.Id) return BadRequest("Id uyuşmuyor");

            var existing = await _actorService.GetByIdAsync(id);
            if (existing is null) return NotFound();

            _mapper.Map(dto, existing);

            await _actorService.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id:guid}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var actor = await _actorService.GetByIdAsync(id);
            if (actor is null) return NotFound();

            await _actorService.DeleteAsync(actor);
            return NoContent();
        }


        [HttpGet("{id:guid}/movies"), AllowAnonymous]
        public async Task<IActionResult> GetMoviesByActor(Guid id)
        {
            var movies = await _actorService.GetMoviesByActorAsync(id);
            if (movies == null) return NotFound();

            var dto = _mapper.Map<IEnumerable<MovieListDto>>(movies);
            return Ok(dto);
        }

        [HttpPut("{id:guid}/rating"), Authorize]
        public async Task<IActionResult> RateActor(Guid id, [FromBody] ScoreDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _actorRatingService.UpsertRatingAsync(id, userId, dto.Score);
            var avg = await _actorRatingService.GetAverageRatingAsync(id);
            return Ok(new { average = avg });
        }

        [HttpGet("{id:guid}/rating"), AllowAnonymous]
        public async Task<IActionResult> GetAverageRating(Guid id)
        {
            var avg = await _actorRatingService.GetAverageRatingAsync(id);
            return Ok(new { average = avg });
        }
    }
}
