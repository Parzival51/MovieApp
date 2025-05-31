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
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        private readonly MovieAppDbContext _ctx;

        public RatingRepository(MovieAppDbContext ctx) : base(ctx) => _ctx = ctx;

        public override async Task<IEnumerable<Rating>> GetAllAsync() =>
            await _ctx.Ratings
                      .Include(r => r.User)
                      .ToListAsync();

        public Task<Rating?> GetByMovieAndUserAsync(Guid movieId, Guid userId) =>
            _ctx.Ratings
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.UserId == userId);

        public async Task<IEnumerable<Rating>> GetByMovieAsync(Guid movieId) =>
            await _ctx.Ratings
                      .Include(r => r.User)
                      .Where(r => r.MovieId == movieId)
                      .ToListAsync();
    }
}
