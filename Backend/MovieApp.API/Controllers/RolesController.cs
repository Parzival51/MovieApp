using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.RoleDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IGenericService<Role> _roleService;
        private readonly IMapper _mapper;

        public RolesController(IGenericService<Role> roleService, IMapper mapper)
        {
            _roleService = roleService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _roleService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<RoleListDto>>(roles);
            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            var dto = _mapper.Map<RoleDetailDto>(role);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            var created = await _roleService.CreateAsync(role);
            var resultDto = _mapper.Map<RoleDetailDto>(created);
            return CreatedAtAction(nameof(GetById), new { id = resultDto.Id }, resultDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoleDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id mismatch");

            var role = _mapper.Map<Role>(dto);
            await _roleService.UpdateAsync(role);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);
            if (role == null)
                return NotFound();

            await _roleService.DeleteAsync(role);
            return NoContent();
        }
    }
}
