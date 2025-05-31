using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.ActorDtos;
using MovieApp.DTO.DTOs.DirectorDtos;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using System.Security.Claims;
using ScoreDto = MovieApp.DTO.DTOs.DirectorDtos.ScoreDto;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly IDirectorService _directorService;
        private readonly IDirectorRatingService _directorRatingService;
        private readonly IMapper _mapper;

        public DirectorsController(IDirectorService directorService, IMapper mapper, IDirectorRatingService directorRatingService)
        {
            _directorService = directorService;
            _directorRatingService = directorRatingService;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var directors = await _directorService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<DirectorListDto>>(directors);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var director = await _directorService.GetByIdAsync(id);
            if (director == null)
                return NotFound();

            var dto = _mapper.Map<DirectorDetailDto>(director);
            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateDirectorDto dto)
        {
            var director = _mapper.Map<Director>(dto);
            var created = await _directorService.CreateAsync(director);
            var resultDto = _mapper.Map<DirectorDetailDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDirectorDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch");

            var director = _mapper.Map<Director>(dto);
            await _directorService.UpdateAsync(director);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var director = await _directorService.GetByIdAsync(id);
            if (director == null)
                return NotFound();

            await _directorService.DeleteAsync(director);
            return NoContent();
        }

        [HttpPut("{id:guid}/rating"), Authorize]
        public async Task<IActionResult> RateDirector(Guid id, [FromBody] ScoreDto dto)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _directorRatingService.UpsertRatingAsync(id, userId, dto.Score);
            var avg = await _directorRatingService.GetAverageRatingAsync(id);
            return Ok(new { average = avg });
        }

        [HttpGet("{id:guid}/rating"), AllowAnonymous]
        public async Task<IActionResult> GetAverageRating(Guid id)
        {
            var avg = await _directorRatingService.GetAverageRatingAsync(id);
            return Ok(new { average = avg });
        }


        [HttpGet("{id:guid}/movies"), AllowAnonymous]
        public async Task<IActionResult> GetMoviesByDirector(Guid id)
        {
            var movies = await _directorService.GetMoviesByDirectorAsync(id);
            if (movies == null)
                return NotFound();

            var dtos = _mapper.Map<IEnumerable<MovieListDto>>(movies);
            return Ok(dtos);
        }
    }
}
