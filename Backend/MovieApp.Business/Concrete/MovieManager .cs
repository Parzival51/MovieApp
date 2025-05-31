// MovieApp.Business.Concrete/MovieManager.cs
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Abstract;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class MovieManager : GenericManager<Movie>, IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;

        public MovieManager(
            IMovieRepository movieRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IMemoryCache cache)
            : base(movieRepository, httpContextAccessor)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _cache = cache;
        }

        public Task<Movie> GetMovieWithDetailsAsync(Guid id) =>
            _movieRepository.GetMovieWithDetailsAsync(id);

        public Task<IEnumerable<Movie>> GetTopRatedMoviesAsync(int count) =>
            _movieRepository.GetTopRatedMoviesAsync(count);

        public async Task<IEnumerable<MovieListDto>> SuggestAsync(string prefix, int maxResults = 5)
        {
            var movies = await _movieRepository.SuggestAsync(prefix, maxResults);
            return _mapper.Map<IEnumerable<MovieListDto>>(movies);
        }

        public async Task<SearchResultDto<MovieListDto>> SearchAsync(string term, int page = 1, int pageSize = 20)
        {
            var cacheKey = $"search:{term}:{page}:{pageSize}";
            if (!_cache.TryGetValue(cacheKey, out SearchResultDto<MovieListDto> result))
            {
                result = await _movieRepository.SearchAsync(term, page, pageSize);
                _cache.Set(cacheKey, result, TimeSpan.FromSeconds(30));
            }
            return result;
        }

        public async Task UpdateMovieAsync(UpdateMovieDto dto)
        {
            var entity = await _movieRepository.GetMovieWithDetailsAsync(dto.Id);
            if (entity == null)
                throw new KeyNotFoundException("Movie not found");

            entity.Title = dto.Title;
            entity.Description = dto.Description;
            entity.ReleaseDate = dto.ReleaseDate;
            entity.Duration = dto.Duration;
            entity.Language = dto.Language;
            entity.Country = dto.Country;
            entity.PosterUrl = dto.PosterUrl;
            entity.TrailerUrl = dto.TrailerUrl;
            entity.Tagline = dto.Tagline;
            entity.Status = dto.Status;
            entity.Budget = dto.Budget;
            entity.Revenue = dto.Revenue;
            entity.ImdbId = dto.ImdbId;
            entity.HomepageUrl = dto.HomepageUrl;
            entity.IsAdult = dto.IsAdult;
            entity.BackdropPath = dto.BackdropPath;

            await _movieRepository.SaveChangesAsync();
        }

        public async Task<Movie> UpsertFromExternalAsync(CreateMovieDto dto)
        {
            var existing = await _movieRepository.GetByExternalIdAsync(dto.ExternalId );
            if (existing != null)
            {
                var distinctCast = dto.Cast.DistinctBy(c => (c.ActorId, c.Order)).ToList();
                var distinctGenres = dto.GenreIds.Distinct().ToList();
                var distinctDirectors = dto.DirectorIds.Distinct().ToList();
                var distinctLangs = dto.SpokenLanguageCodes.Distinct().ToList();

                existing.Title = dto.Title;
                existing.Description = dto.Description;
                existing.ReleaseDate = dto.ReleaseDate;
                existing.Duration = dto.Duration;
                existing.Language = dto.Language;
                existing.Country = dto.Country;
                existing.PosterUrl = dto.PosterUrl;
                existing.TrailerUrl = dto.TrailerUrl;
                existing.Tagline = dto.Tagline;
                existing.Status = dto.Status;
                existing.Budget = dto.Budget;
                existing.Revenue = dto.Revenue;
                existing.ImdbId = dto.ImdbId;
                existing.HomepageUrl = dto.HomepageUrl;
                existing.IsAdult = dto.IsAdult;
                existing.BackdropPath = dto.BackdropPath;

                existing.MovieActors.Clear();
                foreach (var c in distinctCast)
                    existing.MovieActors.Add(new MovieActor { MovieId = existing.Id, ActorId = c.ActorId, Character = c.Character ?? string.Empty, Order = c.Order });

                existing.MovieGenres.Clear();
                foreach (var gid in distinctGenres)
                    existing.MovieGenres.Add(new MovieGenre { MovieId = existing.Id, GenreId = gid });

                existing.MovieDirectors.Clear();
                foreach (var did in distinctDirectors)
                    existing.MovieDirectors.Add(new MovieDirector { MovieId = existing.Id, DirectorId = did });

                existing.MovieLanguages.Clear();
                foreach (var code in distinctLangs)
                    existing.MovieLanguages.Add(new MovieLanguage { MovieId = existing.Id, Iso639 = code });

                existing.Images.Clear();
                foreach (var meta in dto.ImageMetas)
                    existing.Images.Add(new MovieImage
                    {
                        MovieId = existing.Id,
                        Type = meta.Type,
                        FilePath = meta.FilePath,
                        Width = meta.Width,
                        Height = meta.Height,
                        VoteAverage = meta.VoteAverage
                    });

                _movieRepository.Update(existing);
                await _movieRepository.SaveChangesAsync();
                return existing;
            }
            else
            {
                var entity = new Movie
                {
                    ExternalId = dto.ExternalId,
                    Title = dto.Title,
                    Description = dto.Description,
                    ReleaseDate = dto.ReleaseDate,
                    Duration = dto.Duration,
                    Language = dto.Language,
                    Country = dto.Country,
                    PosterUrl = dto.PosterUrl,
                    TrailerUrl = dto.TrailerUrl,
                    Tagline = dto.Tagline,
                    Status = dto.Status,
                    Budget = dto.Budget,
                    Revenue = dto.Revenue,
                    ImdbId = dto.ImdbId,
                    HomepageUrl = dto.HomepageUrl,
                    IsAdult = dto.IsAdult,
                    BackdropPath = dto.BackdropPath,
                };

                foreach (var c in dto.Cast.DistinctBy(c => (c.ActorId, c.Order)))
                    entity.MovieActors.Add(new MovieActor { ActorId = c.ActorId, Character = c.Character ?? string.Empty, Order = c.Order });

                foreach (var gid in dto.GenreIds.Distinct())
                    entity.MovieGenres.Add(new MovieGenre { GenreId = gid });

                foreach (var did in dto.DirectorIds.Distinct())
                    entity.MovieDirectors.Add(new MovieDirector { DirectorId = did });

                foreach (var code in dto.SpokenLanguageCodes.Distinct())
                    entity.MovieLanguages.Add(new MovieLanguage { Iso639 = code });

                foreach (var meta in dto.ImageMetas)
                    entity.Images.Add(new MovieImage
                    {
                        Type = meta.Type,
                        FilePath = meta.FilePath,
                        Width = meta.Width,
                        Height = meta.Height,
                        VoteAverage = meta.VoteAverage
                    });

                await _movieRepository.AddAsync(entity);
                await _movieRepository.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<IEnumerable<MovieListDto>> GetSimilarMoviesAsync(Guid movieId, int max = 5)
        {
            var movies = await _movieRepository.GetSimilarMoviesAsync(movieId, max);
            return _mapper.Map<IEnumerable<MovieListDto>>(movies);
        }

        public async Task<SearchResultDto<MovieListDto>> SearchByGenreAsync(Guid genreId, int page, int pageSize)
        {
            var result = await _movieRepository.SearchByGenreAsync(genreId, page, pageSize);
            return _mapper.Map<SearchResultDto<MovieListDto>>(result);
        }
    }
}
