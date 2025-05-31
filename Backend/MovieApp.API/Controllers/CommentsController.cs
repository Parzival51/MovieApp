using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.CommentDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public CommentsController(
            ICommentService commentService,
            IReviewService reviewService,
            IMapper mapper)
        {
            _commentService = commentService;
            _reviewService = reviewService;
            _mapper = mapper;
        }

        // GET: api/comments?reviewId={guid}
        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GetAll([FromQuery] Guid? reviewId)
        {
            IEnumerable<Comment> comments;
            if (reviewId.HasValue)
                comments = await _commentService.GetFilteredListAsync(c => c.ReviewId == reviewId.Value);
            else
                comments = await _commentService.GetAllAsync();

            return Ok(_mapper.Map<IEnumerable<CommentListDto>>(comments));
        }

        // GET: api/comments/{id}
        [HttpGet("{id:guid}"), AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null) return NotFound();
            var dto = _mapper.Map<CommentDetailDto>(comment);
            return Ok(dto);
        }

        // POST: api/comments
        [HttpPost, Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCommentDto dto)
        {
            var review = await _reviewService.GetByIdAsync(dto.ReviewId);
            if (review == null)
                return NotFound($"Review with ID {dto.ReviewId} not found.");

            var comment = _mapper.Map<Comment>(dto);
            var created = await _commentService.CreateAsync(comment);
            var full = await _commentService.GetByIdWithDetailsAsync(created.Id);
            var resultDto = _mapper.Map<CommentDetailDto>(full);

            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        // PUT: api/comments/{id}
        [HttpPut("{id:guid}"), Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCommentDto dto)
        {
            if (id != dto.Id) return BadRequest("Id mismatch");
            var comment = _mapper.Map<Comment>(dto);
            await _commentService.UpdateAsync(comment);
            return NoContent();
        }

        // DELETE: api/comments/{id}
        [HttpDelete("{id:guid}"), Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null) return NotFound();
            await _commentService.DeleteAsync(comment);
            return NoContent();
        }
        [HttpGet("pending"), Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _commentService.GetFilteredListAsync(c => !c.IsApproved);
            return Ok(_mapper.Map<IEnumerable<CommentListDto>>(list));
        }

        // PUT: api/comments/{id}/approve
        [HttpPut("{id:guid}/approve"), Authorize(Roles = "Admin,Moderator")]
        public async Task<IActionResult> Approve(Guid id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null) return NotFound();

            comment.IsApproved = true;
            comment.UpdatedAt = DateTime.UtcNow;
            await _commentService.UpdateAsync(comment);
            return NoContent();
        } 
    }
}
