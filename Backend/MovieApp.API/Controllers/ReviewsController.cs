using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.ReviewDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviews;
        private readonly IMovieService _movies;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewService reviews, IMovieService movies, IMapper mapper)
        {
            _reviews = reviews;
            _movies = movies;
            _mapper = mapper;
        }


        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var list = await _reviews.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ReviewListDto>>(list));
        }

        [HttpGet("pending"), Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _reviews.GetPendingReviewsAsync();
            return Ok(_mapper.Map<IEnumerable<ReviewListDto>>(list));
        }

        [HttpGet("movie/{movieId:guid}"), AllowAnonymous]
        public async Task<IActionResult> GetByMovie(Guid movieId)
        {
            var list = await _reviews.GetReviewsByMovieAsync(movieId);
            return Ok(_mapper.Map<IEnumerable<ReviewListDto>>(list));
        }


        [HttpGet("{id:guid}"), AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var rev = await _reviews.GetByIdWithDetailsAsync(id);
            return rev is null
                ? NotFound()
                : Ok(_mapper.Map<ReviewDetailDto>(rev));
        }


        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] CreateReviewDto dto)
        {
            if (await _movies.GetByIdAsync(dto.MovieId) is null)
                return NotFound("Movie not found");

            var entity = _mapper.Map<Review>(dto);
            var created = await _reviews.CreateAsync(entity);
            var full = await _reviews.GetByIdWithDetailsAsync(created.Id);

            return CreatedAtAction(nameof(GetById),
                                   new { id = full.Id },
                                   _mapper.Map<ReviewDetailDto>(full));
        }


        [HttpPut("{id:guid}"), Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewDto dto)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");

            await _reviews.UpdateAsync(_mapper.Map<Review>(dto));
            return NoContent();
        }


        [HttpDelete("{id:guid}"), Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var rev = await _reviews.GetByIdAsync(id);
            if (rev is null) return NotFound();

            await _reviews.DeleteAsync(rev);
            return NoContent();
        }

        [HttpPut("{id:guid}/approve"), Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var review = await _reviews.GetByIdAsync(id);
            if (review is null) return NotFound();

            review.IsApproved = true;
            review.UpdatedAt = DateTime.UtcNow;
            await _reviews.UpdateAsync(review);
            return NoContent();
        }
    }
}
