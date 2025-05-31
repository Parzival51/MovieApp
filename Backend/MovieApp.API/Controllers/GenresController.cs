using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.GenreDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenericService<Genre> _genreService;
        private readonly IMapper _mapper;

        public GenresController(
            IGenericService<Genre> genreService,
            IMapper mapper)
        {
            _genreService = genreService;
            _mapper = mapper;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreService.GetAllAsync();
            var dtos = _mapper.Map<List<GenreListDto>>(genres);
            return Ok(dtos);
        }

        // GET: api/Genres/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null) return NotFound();

            var dto = _mapper.Map<GenreDetailDto>(genre);
            return Ok(dto);
        }

        // POST: api/Genres
        // Admin yetkisi gerekli
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGenreDto dto)
        {
            var genre = _mapper.Map<Genre>(dto);
            var created = await _genreService.CreateAsync(genre);

            var resultDto = _mapper.Map<GenreDetailDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, resultDto);
        }

        // PUT: api/Genres/{id}
        // Admin yetkisi gerekli
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateGenreDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch");

            // ① Mevcut entity’yi çek
            var existing = await _genreService.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            // ② Sadece gerekli alanları güncelle
            existing.Name = dto.Name;

            // ③ Kaydet
            await _genreService.UpdateAsync(existing);

            return NoContent();
        }


        // DELETE: api/Genres/{id}
        // Admin yetkisi gerekli
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var genre = await _genreService.GetByIdAsync(id);
            if (genre == null)
                return NotFound();

            await _genreService.DeleteAsync(genre);
            return NoContent();
        }
    }
}
