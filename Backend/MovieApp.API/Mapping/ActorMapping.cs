// MovieApp.API/Mapping/ActorDtoMapping.cs
using AutoMapper;
using MovieApp.Entity.Entities;
using MovieApp.DTO.DTOs.ActorDtos;

namespace MovieApp.API.Mapping
{
    public class ActorDtoMapping : Profile
    {
        public ActorDtoMapping()
        {
            CreateMap<Actor, ActorListDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s =>
                    s.ProfilePath != null
                        ? "https://image.tmdb.org/t/p/w500" + s.ProfilePath
                        : null
                ))
                .ForMember(d => d.Biography, o => o.MapFrom(s => s.Biography))
                .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.Birthday))
                .ForMember(d => d.PlaceOfBirth, o => o.MapFrom(s => s.PlaceOfBirth));

            CreateMap<Actor, ActorDetailDto>()
                .IncludeBase<Actor, ActorListDto>()
                .ForMember(d => d.ExternalId, o => o.MapFrom(s => s.ExternalId))
                .ForMember(d => d.Popularity, o => o.MapFrom(s => s.Popularity))
                .ForMember(d => d.AlsoKnownAs, o => o.Ignore())
                .ForMember(d => d.KnownFor, o => o.Ignore());
        }
    }
}
