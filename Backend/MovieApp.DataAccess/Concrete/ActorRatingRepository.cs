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
    public class ActorRatingRepository : GenericRepository<ActorRating>, IActorRatingRepository
    {
        public ActorRatingRepository(MovieAppDbContext context)
            : base(context) { }

        public Task<ActorRating> GetUserRatingAsync(Guid actorId, Guid userId) =>
            _dbSet
              .FirstOrDefaultAsync(r => r.ActorId == actorId && r.UserId == userId);

        public Task<IEnumerable<ActorRating>> GetRatingsByActorAsync(Guid actorId) =>
            _dbSet
              .Where(r => r.ActorId == actorId)
              .ToListAsync()
              .ContinueWith(t => (IEnumerable<ActorRating>)t.Result);
    }
}
