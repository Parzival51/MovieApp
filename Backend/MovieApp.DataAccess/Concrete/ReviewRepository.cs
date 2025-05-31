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
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly MovieAppDbContext _ctx;
        public ReviewRepository(MovieAppDbContext ctx) : base(ctx) => _ctx = ctx;

        public Task<IEnumerable<Review>> GetPendingReviewsAsync() =>
            _ctx.Reviews
                .Include(r => r.User)
                .Where(r => !r.IsApproved)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<Review>)t.Result);

        public Task<IEnumerable<Review>> GetReviewsByMovieAsync(Guid movieId) =>
            _ctx.Reviews
                .Include(r => r.User)
                .Where(r => r.MovieId == movieId && r.IsApproved)
                .ToListAsync()
                .ContinueWith(t => (IEnumerable<Review>)t.Result);

        public async Task<Review> GetByIdWithDetailsAsync(Guid id) =>
            await _ctx.Reviews
                      .Include(r => r.User)
                      .Include(r => r.Comments)
                          .ThenInclude(c => c.User)
                      .FirstOrDefaultAsync(r => r.Id == id);
    }
}
