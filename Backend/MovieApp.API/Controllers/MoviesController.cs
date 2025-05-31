// src/Controllers/MoviesController.cs
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;

        public MoviesController(
            IMovieService movieService,
            IMapper mapper)
        {
            _movieService = movieService;
            _mapper = mapper;
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<MovieListDto>>(movies));
        }

        [HttpGet("top-rated"), AllowAnonymous]
        public async Task<IActionResult> GetTopRated([FromQuery] int count = 5)
        {
            var movies = await _movieService.GetTopRatedMoviesAsync(count);
            return Ok(_mapper.Map<IEnumerable<MovieListDto>>(movies));
        }

        [HttpGet("{id}"), AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var movie = await _movieService.GetMovieWithDetailsAsync(id);
            if (movie == null) return NotFound();
            return Ok(_mapper.Map<MovieDetailDto>(movie));
        }

        [HttpPost, Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateMovieDto dto)
        {
            var entity = _mapper.Map<Movie>(dto);
            await _movieService.CreateAsync(entity);

            var created = await _movieService.GetMovieWithDetailsAsync(entity.Id);
            return CreatedAtAction(
                nameof(GetById),
                new { id = created.Id },
                _mapper.Map<MovieDetailDto>(created)
            );
        }

        [HttpPut("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateMovieDto dto)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");
            await _movieService.UpdateMovieAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _movieService.GetByIdAsync(id);
            if (movie == null) return NotFound();
            await _movieService.DeleteAsync(movie);
            return NoContent();
        }

        [HttpGet("{id}/similar"), AllowAnonymous]
        public async Task<IActionResult> GetSimilar(Guid id, [FromQuery] int max = 5)
        {
            var sims = await _movieService.GetSimilarMoviesAsync(id, max);
            return Ok(sims);
        }

        [HttpGet("search"), AllowAnonymous]
        public async Task<IActionResult> Search(
            [FromQuery] string? q,
            [FromQuery] Guid? genre,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (!string.IsNullOrWhiteSpace(q))
            {
                var byText = await _movieService.SearchAsync(q, page, pageSize);
                return Ok(byText);
            }
            else if (genre.HasValue)
            {
                var byGenre = await _movieService.SearchByGenreAsync(genre.Value, page, pageSize);
                return Ok(byGenre);
            }
            else
            {
                var all = await _movieService.GetAllAsync();
                var dtos = _mapper.Map<IEnumerable<MovieListDto>>(all);
                return Ok(new SearchResultDto<MovieListDto>
                {
                    Items = dtos,
                    Total = dtos.Count()
                });
            }
        }
    }
}
