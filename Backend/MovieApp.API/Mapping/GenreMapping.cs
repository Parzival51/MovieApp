using AutoMapper;
using MovieApp.DTO.DTOs.GenreDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class GenreMapping : Profile
    {
        public GenreMapping()
        {
            CreateMap<CreateGenreDto, Genre>();
            CreateMap<UpdateGenreDto, Genre>();
            CreateMap<Genre, GenreListDto>();
            CreateMap<Genre, GenreDetailDto>();
        }
    }
}
