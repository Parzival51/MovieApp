using AutoMapper;
using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.DTO.DTOs.MovieDtos;
using System.Globalization;

namespace MovieApp.API.Mapping
{
    public class TmdbMapping : Profile
    {
        public TmdbMapping()
        {
            CreateMap<TmdbFullMovieDto, CreateMovieDto>()
                .ForMember(d => d.Title,
                           o => o.MapFrom(s => s.title))
                .ForMember(d => d.Description,
                           o => o.MapFrom(s => s.overview))
                .ForMember(d => d.ReleaseDate,
                           o => o.MapFrom(s =>
                               DateTime.ParseExact(s.release_date, "yyyy-MM-dd", CultureInfo.InvariantCulture)))
                .ForMember(d => d.Language,
                           o => o.MapFrom(s => s.original_language))
                .ForMember(d => d.Country,
                           o => o.MapFrom(s =>
                               s.production_countries.Select(pc => pc.iso_3166_1).FirstOrDefault()))
                .ForMember(d => d.PosterUrl,
                           o => o.MapFrom(s =>
                               string.IsNullOrEmpty(s.poster_path)
                                   ? null
                                   : $"https://image.tmdb.org/t/p/w500{s.poster_path}"))
                .ForMember(d => d.Duration,
                           o => o.MapFrom(s => s.runtime))      
                                                              
                .ForMember(d => d.TrailerUrl,
                           o => o.MapFrom(s =>
                               s.videos.results
                                .Where(v => v.site == "YouTube" && v.type == "Trailer")
                                .Select(v => $"https://www.youtube.com/watch?v={v.key}")
                                .FirstOrDefault()))
                .ForMember(d => d.Tagline,
                           o => o.MapFrom(s => s.tagline))
                .ForMember(d => d.Status,
                           o => o.MapFrom(s => s.status))
                .ForMember(d => d.Budget,
                           o => o.MapFrom(s => s.budget))
                .ForMember(d => d.Revenue,
                           o => o.MapFrom(s => s.revenue))
                .ForMember(d => d.ImdbId,
                           o => o.MapFrom(s => s.imdb_id))
                .ForMember(d => d.HomepageUrl,
                           o => o.MapFrom(s => s.homepage))
                .ForMember(d => d.IsAdult,
                           o => o.MapFrom(s => s.adult))
                .ForMember(d => d.BackdropPath,
                           o => o.MapFrom(s => s.backdrop_path))

                .ForMember(d => d.GenreIds, o => o.Ignore())
                .ForMember(d => d.ActorIds, o => o.Ignore())
                .ForMember(d => d.DirectorIds, o => o.Ignore());
        }
    }

}
