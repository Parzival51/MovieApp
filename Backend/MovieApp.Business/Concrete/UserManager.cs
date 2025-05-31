using AutoMapper;
using Microsoft.AspNetCore.Identity;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.AuthDtos;
using MovieApp.DTO.DTOs.UserDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class UserManagerService : IUserService
    {
        private readonly UserManager<User> _userMgr;
        private readonly RoleManager<Role> _roleMgr;
        private readonly IMapper _mapper;

        public UserManagerService(
            UserManager<User> userMgr,
            RoleManager<Role> roleMgr,
            IMapper mapper)
        {
            _userMgr = userMgr;
            _roleMgr = roleMgr;
            _mapper = mapper;
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var result = await _userMgr.CreateAsync(user, dto.Password);
            if (result.Succeeded && dto.Roles.Any())
            {
                await _userMgr.AddToRolesAsync(user, dto.Roles);
            }
            return result;
        }

        public async Task<bool> AssignRolesAsync(AssignRolesDto dto)
        {
            var user = await _userMgr.FindByIdAsync(dto.UserId.ToString());
            if (user == null) return false;

            var currentRoles = await _userMgr.GetRolesAsync(user);
            await _userMgr.RemoveFromRolesAsync(user, currentRoles);

            var validRoles = new List<string>();
            foreach (var role in dto.Roles)
            {
                if (await _roleMgr.RoleExistsAsync(role))
                {
                    validRoles.Add(role);
                }
            }

            var addResult = await _userMgr.AddToRolesAsync(user, validRoles);
            return addResult.Succeeded;
        }

        public async Task<List<UserListDto>> GetUsersInRoleAsync(string roleName)
        {
            var usersInRole = await _userMgr.GetUsersInRoleAsync(roleName);
            return _mapper.Map<List<UserListDto>>(usersInRole);
        }

        public async Task<List<UserListDto>> GetAllUsersAsync()
        {
            var users = _userMgr.Users.ToList();
            var list = _mapper.Map<List<UserListDto>>(users);

            foreach (var dto in list)
            {
                var roles = await _userMgr.GetRolesAsync(
                                users.First(u => u.Id == dto.Id));
                dto.Roles = roles;
            }
            return list;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _userMgr.FindByIdAsync(id.ToString());
            if (user == null) return false;

            var result = await _userMgr.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task UpdateRolesAsync(Guid userId, IList<string> roles)
        {
            var user = await _userMgr.FindByIdAsync(userId.ToString());
            if (user == null) throw new KeyNotFoundException("User not found");

            var current = await _userMgr.GetRolesAsync(user);
            var toAdd = roles.Except(current);
            var toRemove = current.Except(roles);

            await _userMgr.RemoveFromRolesAsync(user, toRemove);
            await _userMgr.AddToRolesAsync(user, toAdd);
        }




    }

}
