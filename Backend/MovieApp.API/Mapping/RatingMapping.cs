using AutoMapper;
using MovieApp.DTO.DTOs.RatingDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class RatingMapping : Profile
    {
        public RatingMapping()
        {
            CreateMap<CreateRatingDto, Rating>();
            CreateMap<UpdateRatingDto, Rating>();

            /* null-güvenli UserName */
            CreateMap<Rating, RatingListDto>()
                .ForMember(d => d.UserName,
                           o => o.MapFrom(s =>
                               s.User == null
                                 ? string.Empty
                                 : (s.User.DisplayName ?? s.User.UserName)));

            CreateMap<Rating, RatingDetailDto>()
                .IncludeBase<Rating, RatingListDto>();
        }
    }
}
