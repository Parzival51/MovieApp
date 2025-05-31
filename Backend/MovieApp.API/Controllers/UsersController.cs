using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.AuthDtos;
using MovieApp.DTO.DTOs.UserDtos;
using System;
using System.Threading.Tasks;

namespace MovieApp.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        /* ────────────────────────────────────────────────────────────────
           1. Yeni kullanıcı oluştur
        ───────────────────────────────────────────────────────────────── */
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

        /* ────────────────────────────────────────────────────────────────
           2. ROLLERİ GÜNCELLE  (PUT /users/{id}/roles)
        ───────────────────────────────────────────────────────────────── */
        [HttpPut("{userId:guid}/roles")]
        public async Task<IActionResult> UpdateRoles(Guid userId,
            [FromBody] UpdateUserRolesDto dto)
        {
            await _userService.UpdateRolesAsync(userId, dto.Roles);
            return NoContent();
        }

        /* ────────────────────────────────────────────────────────────────
           3. Belirli roldeki kullanıcıları listele
        ───────────────────────────────────────────────────────────────── */
        [HttpGet("role/{roleName}")]
        public async Task<IActionResult> GetByRole(string roleName)
        {
            var list = await _userService.GetUsersInRoleAsync(roleName);
            return Ok(list);
        }

        /* ────────────────────────────────────────────────────────────────
           4. Tüm kullanıcıları listele
        ───────────────────────────────────────────────────────────────── */
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _userService.GetAllUsersAsync();
            return Ok(list);
        }

        /* ────────────────────────────────────────────────────────────────
           5. Kullanıcı sil
        ───────────────────────────────────────────────────────────────── */
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _userService.DeleteUserAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
