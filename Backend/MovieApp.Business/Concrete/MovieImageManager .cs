using Microsoft.AspNetCore.Http;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Abstract;
using MovieApp.DataAccess.Concrete;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class MovieImageManager : GenericManager<MovieImage>, IMovieImageService
    {
        private readonly IMovieImageRepository _repo;

        public MovieImageManager(
            IMovieImageRepository repo,
            IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
            _repo = repo;
        }

        public Task<IEnumerable<MovieImage>> GetByMovieAsync(Guid movieId) =>
            _repo.GetByMovieAsync(movieId);
    }
}
