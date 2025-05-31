using AutoMapper;
using MovieApp.DTO.DTOs.FavoriteDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class FavoriteMapping : Profile
    {
        public FavoriteMapping()
        {
            CreateMap<CreateFavoriteDto, Favorite>();
            CreateMap<Favorite, FavoriteListDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie.Title));
            CreateMap<Favorite, FavoriteDetailDto>().IncludeBase<Favorite, FavoriteListDto>();
        }
    }
}
