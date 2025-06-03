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

       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var result = await _userService.CreateUserAsync(dto);
            return result.Succeeded ? Ok() : BadRequest(result.Errors);
        }

    
        [HttpPut("{userId:guid}/roles")]
        public async Task<IActionResult> UpdateRoles(Guid userId,
            [FromBody] UpdateUserRolesDto dto)
        {
            await _userService.UpdateRolesAsync(userId, dto.Roles);
            return NoContent();
        }

     
        [HttpGet("role/{roleName}")]
        public async Task<IActionResult> GetByRole(string roleName)
        {
            var list = await _userService.GetUsersInRoleAsync(roleName);
            return Ok(list);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _userService.GetAllUsersAsync();
            return Ok(list);
        }

   
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await _userService.DeleteUserAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
