using AutoMapper;
using MovieApp.DTO.DTOs.RoleDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{

    public class RoleMapping : Profile
    {
        public RoleMapping()
        {
            CreateMap<CreateRoleDto, Role>();
            CreateMap<UpdateRoleDto, Role>();
            CreateMap<Role, RoleListDto>();
            CreateMap<Role, RoleDetailDto>()
                .ForMember(dest => dest.AssignedUsers, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.User.UserName)));
        }
    }
}
