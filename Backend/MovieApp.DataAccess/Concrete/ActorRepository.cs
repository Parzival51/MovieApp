using Microsoft.EntityFrameworkCore;
using MovieApp.DataAccess.Abstract;
using MovieApp.DataAccess.Context;
using MovieApp.DataAccess.Repositories;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Concrete
{
    public class ActorRepository : GenericRepository<Actor>, IActorRepository
    {
        private readonly MovieAppDbContext _ctx;

        public ActorRepository(MovieAppDbContext ctx)
            : base(ctx)
        {
            _ctx = ctx;
        }

        public Task<Actor?> GetByExternalIdAsync(int externalId) =>
            _ctx.Actors
                .FirstOrDefaultAsync(a => a.ExternalId == externalId);

        public async Task<IEnumerable<Movie>> GetMoviesByActorAsync(Guid actorId)
        {
            return await _ctx.Movies
                             .Include(m => m.MovieActors)
                             .Include(m => m.Ratings)         
                             .Where(m => m.MovieActors.Any(ma => ma.ActorId == actorId))
                             .ToListAsync();
        }
    }
}
