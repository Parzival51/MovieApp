// src/Mapping/DirectorMapping.cs
using AutoMapper;
using MovieApp.DTO.DTOs.DirectorDtos;
using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class DirectorMapping : Profile
    {
        public DirectorMapping()
        {
            CreateMap<TmdbCrewDto, Director>()
                .ForMember(d => d.ExternalId, o => o.MapFrom(s => s.id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.name))
                .ForMember(d => d.ProfilePath, o => o.MapFrom(s => s.profile_path))
                .ForMember(d => d.Popularity, o => o.MapFrom(s => s.popularity))
                .ForAllMembers(o => o.Ignore());

            CreateMap<Director, DirectorListDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.ProfilePath, o => o.MapFrom(s =>
                    s.ProfilePath != null
                      ? "https://image.tmdb.org/t/p/w500" + s.ProfilePath
                      : null))
                .ForMember(d => d.Popularity, o => o.MapFrom(s => s.Popularity));

            CreateMap<Director, DirectorDetailDto>()
                .IncludeBase<Director, DirectorListDto>()
                .ForMember(d => d.Biography, o => o.MapFrom(s => s.Biography))
                .ForMember(d => d.Birthday, o => o.MapFrom(s => s.Birthday))
                .ForMember(d => d.PlaceOfBirth, o => o.MapFrom(s => s.PlaceOfBirth))
                .ForMember(d => d.AlsoKnownAs, o => o.Ignore());
        }
    }
}
