using AutoMapper;
using MovieApp.DTO.DTOs.UserDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UserListDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)));
            CreateMap<User, UserDetailDto>().IncludeBase<User, UserListDto>();
        }
    }
}
