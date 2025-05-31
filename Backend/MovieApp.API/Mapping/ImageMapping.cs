using AutoMapper;
using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class ImageMapping : Profile
    {
        public ImageMapping()
        {
            CreateMap<TmdbImageDto, ImageMetaDto>()
                .ForMember(d => d.FilePath, o => o.MapFrom(s => $"https://image.tmdb.org/t/p/w500{s.file_path}"))
                .ForMember(d => d.Width, o => o.MapFrom(s => (short)s.width))
                .ForMember(d => d.Height, o => o.MapFrom(s => (short)s.height))
                .ForMember(d => d.VoteAverage, o => o.MapFrom(s => (float)s.vote_average))
                .ForMember(d => d.Type, o => o.Ignore()); 
        }
    }
}
