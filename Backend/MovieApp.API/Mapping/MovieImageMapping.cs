using AutoMapper;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class MovieImageMapping : Profile
    {
        public MovieImageMapping()
        {
            CreateMap<MovieImage, MovieImageDto>()
                .ForMember(d => d.Type, o => o.MapFrom(s => s.Type))
                .ForMember(d => d.FilePath, o => o.MapFrom(s => s.FilePath))
                .ForMember(d => d.Width, o => o.MapFrom(s => s.Width))
                .ForMember(d => d.Height, o => o.MapFrom(s => s.Height))
                .ForMember(d => d.VoteAverage, o => o.MapFrom(s => s.VoteAverage));


        }
    }
}
