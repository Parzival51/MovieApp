using AutoMapper;
using MovieApp.DTO.DTOs.ReviewDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class ReviewMapping : Profile
    {
        public ReviewMapping()
        {
            CreateMap<CreateReviewDto, Review>();
            CreateMap<UpdateReviewDto, Review>();

            CreateMap<Review, ReviewListDto>()
                .ForMember(d => d.MovieTitle, o => o.MapFrom(s => s.Movie.Title))
                .ForMember(d => d.UserName, o => o.MapFrom(s =>
                                    s.User.DisplayName ?? s.User.UserName))
                .ForMember(d => d.Content, o => o.MapFrom(s => s.Content));  

            CreateMap<Review, ReviewDetailDto>()
                .IncludeBase<Review, ReviewListDto>()
                .ForMember(d => d.Comments,
                   o => o.MapFrom(s => s.Comments
                                         .Select(c => c.Content)
                                         .ToList()));
        }
    }
}
