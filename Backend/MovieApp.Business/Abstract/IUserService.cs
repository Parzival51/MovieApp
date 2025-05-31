using Microsoft.AspNetCore.Identity;
using MovieApp.DTO.DTOs.AuthDtos;
using MovieApp.DTO.DTOs.UserDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IUserService
    {
        Task<IdentityResult> CreateUserAsync(CreateUserDto dto);
        Task<bool> AssignRolesAsync(AssignRolesDto dto);
        Task<List<UserListDto>> GetUsersInRoleAsync(string roleName);
        Task<List<UserListDto>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(Guid id);

        Task UpdateRolesAsync(Guid userId, IList<string> roles);


    }
}
