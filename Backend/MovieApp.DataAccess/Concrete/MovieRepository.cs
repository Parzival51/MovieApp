using Microsoft.EntityFrameworkCore;
using MovieApp.DataAccess.Abstract;
using MovieApp.DataAccess.Context;
using MovieApp.DataAccess.Repositories;
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Concrete
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly MovieAppDbContext _ctx;

        public MovieRepository(MovieAppDbContext ctx) : base(ctx) => _ctx = ctx;


        public Task<Movie?> GetMovieWithDetailsAsync(Guid id) =>
            _ctx.Movies
                .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
                .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieDirectors).ThenInclude(md => md.Director)
                .Include(m => m.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

    
        public Task<IEnumerable<Movie>> GetTopRatedMoviesAsync(int count) =>
            _ctx.Movies
                .Where(m => m.Ratings.Any())
                .OrderByDescending(m => m.Ratings.Average(r => (double)r.Score10))
                .Take(count)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<Movie>)t.Result);

        public Task<IEnumerable<Movie>> SuggestAsync(string prefix, int max = 5) =>
            _ctx.Movies
                .AsNoTracking()
                .Where(m => EF.Functions.Like(m.Title, $"{prefix}%"))
                .OrderBy(m => m.Title)
                .Take(max)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<Movie>)t.Result);


        public async Task<SearchResultDto<MovieListDto>> SearchAsync(
            string term, int page = 1, int pageSize = 20)
        {
            var qry = _ctx.Movies.AsNoTracking()
                         .Where(m =>
                             EF.Functions.Contains(m.Title, $"\"{term}*\"") ||
                             EF.Functions.Contains(m.Description, $"\"{term}*\""));

            var total = await qry.CountAsync();

            var items = await qry.OrderBy(m => m.Title)
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .Select(m => new MovieListDto
                                 {
                                     Id = m.Id,
                                     Title = m.Title,
                                     ReleaseDate = m.ReleaseDate,
                                     PosterUrl = m.PosterUrl,
                                     AverageRating = m.Ratings.Any()
                                                       ? m.Ratings.Average(r => (double)r.Score10)
                                                       : 0
                                 })
                                 .ToListAsync();

            return new SearchResultDto<MovieListDto>
            {
                Items = items,
                Total = total
            };

        }

        public Task<Movie?> GetByExternalIdAsync(int externalId) =>
        _ctx.Movies
            .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
            .Include(m => m.MovieActors).ThenInclude(ma => ma.Actor)
            .Include(m => m.MovieDirectors).ThenInclude(md => md.Director)
            .Include(m => m.MovieLanguages).ThenInclude(ml => ml.Language)
            .Include(m => m.Images)
            .FirstOrDefaultAsync(m => m.ExternalId == externalId);

        public async Task<IEnumerable<Movie>> GetSimilarMoviesAsync(Guid movieId, int max = 5)
        {
        
            var genreIds = await _ctx.MovieGenres
                .Where(mg => mg.MovieId == movieId)
                .Select(mg => mg.GenreId)
                .ToListAsync();

            var sims = await _ctx.Movies
                .AsNoTracking()
                .Include(m => m.MovieGenres)
                .Where(m =>
                    m.Id != movieId &&
                    m.MovieGenres.Any(mg => genreIds.Contains(mg.GenreId))
                )
                .OrderByDescending(m =>
                    m.MovieGenres.Count(mg => genreIds.Contains(mg.GenreId))
                )
                .Take(max)
                .ToListAsync();

            return sims;
        }

        public async Task<SearchResultDto<MovieListDto>> SearchByGenreAsync(
        Guid genreId, int page, int pageSize)
        {
            var qry = _ctx.Movies
                .AsNoTracking()
                .Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId));

            var total = await qry.CountAsync();

            var items = await qry
                .OrderBy(m => m.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => new MovieListDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    ReleaseDate = m.ReleaseDate,
                    PosterUrl = m.PosterUrl,
                    AverageRating = m.Ratings.Any()
                        ? m.Ratings.Average(r => r.Score10)
                        : 0,
                    Duration = m.Duration,
                    Language = m.Language,
                    Country = m.Country
                })
                .ToListAsync();

            return new SearchResultDto<MovieListDto>
            {
                Items = items,
                Total = total
            };
        }


    }
}
