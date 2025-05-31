using Microsoft.EntityFrameworkCore;
using MovieApp.DataAccess.Abstract;
using MovieApp.DataAccess.Context;
using MovieApp.DataAccess.Repositories;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Concrete
{
    public class MovieImageRepository : GenericRepository<MovieImage>, IMovieImageRepository
    {
        private readonly MovieAppDbContext _ctx;

        public MovieImageRepository(MovieAppDbContext ctx)
            : base(ctx)
        {
            _ctx = ctx;
        }

        public async Task<IEnumerable<MovieImage>> GetByMovieAsync(Guid movieId) =>
            await _ctx.MovieImages
                      .Where(mi => mi.MovieId == movieId)
                      .ToListAsync();
    }
}
