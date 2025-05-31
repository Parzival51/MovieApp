// MovieApp.API/Mapping/MovieMapping.cs
using AutoMapper;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using System.Linq;

namespace MovieApp.API.Mapping
{
    public class MovieMapping : Profile
    {
        public MovieMapping()
        {
            CreateMap<CreateMovieDto, Movie>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.ExternalId, opt => opt.MapFrom(s => s.ExternalId))
                .ForMember(d => d.MovieGenres, opts =>
                    opts.MapFrom(src =>
                        src.GenreIds.Select(gid => new MovieGenre { GenreId = gid })))
                .ForMember(d => d.MovieActors, opts =>
                    opts.MapFrom(src =>
                        src.Cast.Select(c => new MovieActor
                        {
                            ActorId = c.ActorId,
                            Character = c.Character ?? string.Empty,
                            Order = c.Order
                        })))
                .ForMember(d => d.MovieDirectors, opts =>
                    opts.MapFrom(src =>
                        src.DirectorIds.Select(did => new MovieDirector { DirectorId = did })))
                .ForMember(d => d.MovieLanguages, opts =>
                    opts.MapFrom(src =>
                        src.SpokenLanguageCodes.Select(code => new MovieLanguage { Iso639 = code })))

                .ForMember(d => d.Images, opts =>
                    opts.MapFrom(src =>
                        src.ImageMetas.Select(im => new MovieImage
                        {
                            Type = im.Type,
                            FilePath = im.FilePath,
                            Width = im.Width,
                            Height = im.Height,
                            VoteAverage = im.VoteAverage
                        })));

            CreateMap<UpdateMovieDto, Movie>()
                .ForMember(d => d.Id, ignore => ignore.Ignore())
                .ForMember(d => d.MovieGenres, opts =>
                    opts.MapFrom(src =>
                        src.GenreIds.Select(gid => new MovieGenre { GenreId = gid })))
                .ForMember(d => d.MovieActors, opts =>
                    opts.MapFrom(src =>
                        src.ActorIds.Select(aid => new MovieActor
                        {
                            ActorId = aid,
                            Character = string.Empty,
                            Order = 0
                        })))
                .ForMember(d => d.MovieDirectors, opts =>
                    opts.MapFrom(src =>
                        src.DirectorIds.Select(did => new MovieDirector { DirectorId = did })))
                .ForMember(d => d.MovieLanguages, opts =>
                    opts.MapFrom(src =>
                        src.SpokenLanguageCodes.Select(code => new MovieLanguage { Iso639 = code })))
                .ForAllMembers(opt => opt.Ignore());

            CreateMap<Movie, MovieListDto>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.Title, opt => opt.MapFrom(s => s.Title))
                .ForMember(d => d.ReleaseDate, opt => opt.MapFrom(s => s.ReleaseDate))
                .ForMember(d => d.PosterUrl, opt => opt.MapFrom(s => s.PosterUrl))
                .ForMember(d => d.AverageRating, opt =>
                    opt.MapFrom(s => s.Ratings.Any()
                        ? s.Ratings.Average(r => r.Score10)
                        : 0))
                .ForMember(d => d.Duration, opt => opt.MapFrom(s => s.Duration))
                .ForMember(d => d.Language, opt => opt.MapFrom(s => s.Language))
                .ForMember(d => d.Country, opt => opt.MapFrom(s => s.Country));

            CreateMap<Movie, MovieDetailDto>()
                .IncludeBase<Movie, MovieListDto>()
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description))
                .ForMember(d => d.Tagline, opt => opt.MapFrom(s => s.Tagline))
                .ForMember(d => d.Status, opt => opt.MapFrom(s => s.Status))
                .ForMember(d => d.Budget, opt => opt.MapFrom(s => s.Budget))
                .ForMember(d => d.Revenue, opt => opt.MapFrom(s => s.Revenue))
                .ForMember(d => d.ImdbId, opt => opt.MapFrom(s => s.ImdbId))
                .ForMember(d => d.HomepageUrl, opt => opt.MapFrom(s => s.HomepageUrl))
                .ForMember(d => d.IsAdult, opt => opt.MapFrom(s => s.IsAdult))
                .ForMember(d => d.BackdropPath, opt => opt.MapFrom(s => s.BackdropPath))
                .ForMember(d => d.Genres, opt => opt.MapFrom(s => s.MovieGenres.Select(mg => mg.Genre.Name)))
                .ForMember(d => d.Actors, opt => opt.MapFrom(s => s.MovieActors.Select(ma => ma.Actor.Name)))
                .ForMember(d => d.Directors, opt => opt.MapFrom(s => s.MovieDirectors.Select(md => md.Director.Name)))
                .ForMember(d => d.SpokenLanguages, opt =>
                    opt.MapFrom(s => s.MovieLanguages.Select(ml => new MovieLangDto
                    {
                        Iso639 = ml.Iso639,
                        EnglishName = ml.Language.EnglishName
                    })))
                .ForMember(d => d.Posters, opt =>
                    opt.MapFrom(s => s.Images
                        .Where(i => i.Type == "poster")
                        .Select(i => new ImageMetaDto
                        {
                            Type = i.Type,
                            FilePath = i.FilePath,
                            Width = (short)i.Width,
                            Height = (short)i.Height,
                            VoteAverage = i.VoteAverage
                        })))
                .ForMember(d => d.Backdrops, opt =>
                    opt.MapFrom(s => s.Images
                        .Where(i => i.Type == "backdrop")
                        .Select(i => new ImageMetaDto
                        {
                            Type = i.Type,
                            FilePath = i.FilePath,
                            Width = (short)i.Width,
                            Height = (short)i.Height,
                            VoteAverage = i.VoteAverage
                        })))
                .ForMember(d => d.Cast, opt =>
                    opt.MapFrom(s => s.MovieActors
                        .OrderBy(ma => ma.Order)
                        .Select(ma => new ActorCastDto
                        {
                            ActorId = ma.ActorId,
                            Name = ma.Actor.Name,
                            PhotoUrl = ma.Actor.ProfilePath != null
                                       ? "https://image.tmdb.org/t/p/w500" + ma.Actor.ProfilePath
                                       : null,
                            Character = ma.Character,
                            Order = ma.Order
                        })));
        }
    }
}
