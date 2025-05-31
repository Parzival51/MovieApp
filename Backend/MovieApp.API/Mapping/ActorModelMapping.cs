using AutoMapper;
using MovieApp.DTO.DTOs.ActorDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class ActorModelMapping : Profile
    {
        public ActorModelMapping()
        {
           
            CreateMap<CreateActorDto, Actor>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Birthday, o => o.MapFrom(s => s.BirthDate))
                .ForMember(d => d.Biography, o => o.MapFrom(s => s.Bio))

                .ForMember(d => d.Id, o => o.Ignore())
                .ForMember(d => d.ExternalId, o => o.Ignore())
                .ForMember(d => d.Gender, o => o.Ignore())
                .ForMember(d => d.ProfilePath, o => o.Ignore())
                .ForMember(d => d.Popularity, o => o.Ignore())
                .ForMember(d => d.PlaceOfBirth, o => o.Ignore())
                .ForMember(d => d.MovieActors, o => o.Ignore())
                ;


            CreateMap<UpdateActorDto, Actor>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Birthday, o => o.MapFrom(s => s.BirthDate))
                .ForMember(d => d.Biography, o => o.MapFrom(s => s.Bio))

                .ForMember(d => d.ExternalId, o => o.Ignore())
                .ForMember(d => d.Gender, o => o.Ignore())
                .ForMember(d => d.ProfilePath, o => o.Ignore())
                .ForMember(d => d.Popularity, o => o.Ignore())
                .ForMember(d => d.PlaceOfBirth, o => o.Ignore())
                .ForMember(d => d.MovieActors, o => o.Ignore())
                ;
        }
    }
}
