using AutoMapper;
using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class LanguageMapping : Profile
    {
        public LanguageMapping()
        {
            CreateMap<TmdbLanguageDto, Language>()
                .ForMember(d => d.Iso639, o => o.MapFrom(s => s.iso_639_1))
                .ForMember(d => d.EnglishName, o => o.MapFrom(s => s.english_name))
                .ForAllMembers(o => o.Ignore());

            CreateMap<Language, MovieLangDto>()
                .ForMember(d => d.Iso639, o => o.MapFrom(s => s.Iso639))
                .ForMember(d => d.EnglishName, o => o.MapFrom(s => s.EnglishName));
        }
    }
}
