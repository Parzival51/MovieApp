// MovieApp.Business.Abstract/IMovieService.cs
using MovieApp.DTO.DTOs.MovieDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IMovieService : IGenericService<Movie>
    {
        Task<Movie> GetMovieWithDetailsAsync(Guid id);
        Task<IEnumerable<Movie>> GetTopRatedMoviesAsync(int count);
        Task<IEnumerable<MovieListDto>> SuggestAsync(string prefix, int maxResults = 5);
        Task<SearchResultDto<MovieListDto>> SearchAsync(string term, int page = 1, int pageSize = 20);
        Task UpdateMovieAsync(UpdateMovieDto dto);
        Task<Movie> UpsertFromExternalAsync(CreateMovieDto dto);
        Task<IEnumerable<MovieListDto>> GetSimilarMoviesAsync(Guid movieId, int max = 5);
        Task<SearchResultDto<MovieListDto>> SearchByGenreAsync(Guid genreId, int page, int pageSize);
    }
}
