using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Abstract
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        
        Task<Movie> GetMovieWithDetailsAsync(Guid id);

        Task<IEnumerable<Movie>> GetTopRatedMoviesAsync(int count);

        Task<IEnumerable<Movie>> SuggestAsync(string prefix, int maxResults = 5);

        Task<SearchResultDto<MovieListDto>> SearchAsync(string term, int page = 1, int pageSize = 20); 
        Task<Movie?> GetByExternalIdAsync(int externalId);

        Task<IEnumerable<Movie>> GetSimilarMoviesAsync(Guid movieId, int max = 5);

        Task<SearchResultDto<MovieListDto>> SearchByGenreAsync(Guid genreId, int page, int pageSize);

    }
}

