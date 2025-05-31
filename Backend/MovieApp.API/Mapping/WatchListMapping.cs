using AutoMapper;
using MovieApp.DTO.DTOs.WatchListDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class WatchlistMapping : Profile
    {
        public WatchlistMapping()
        {
            CreateMap<CreateWatchlistDto, Watchlist>();
            CreateMap<Watchlist, WatchlistListDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie.Title));
            CreateMap<Watchlist, WatchlistDetailDto>().IncludeBase<Watchlist, WatchlistListDto>();
        }
    }
}
