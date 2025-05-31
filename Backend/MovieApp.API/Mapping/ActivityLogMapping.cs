using AutoMapper;
using MovieApp.DTO.DTOs.ActivityLogDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class ActivityLogMapping : Profile
    {
        public ActivityLogMapping()
        {
            CreateMap<ActivityLog, ActivityLogListDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName));
        }
    }
}
