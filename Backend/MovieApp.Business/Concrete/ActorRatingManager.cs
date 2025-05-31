using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Abstract;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class ActorRatingManager : IActorRatingService
    {
        private readonly IActorRatingRepository _repo;
        public ActorRatingManager(IActorRatingRepository repo)
        {
            _repo = repo;
        }

        public async Task<ActorRating> GetUserRatingAsync(Guid actorId, Guid userId) =>
            await _repo.GetUserRatingAsync(actorId, userId);

        public async Task<IEnumerable<ActorRating>> GetRatingsByActorAsync(Guid actorId) =>
            await _repo.GetRatingsByActorAsync(actorId);

        public async Task<double> GetAverageRatingAsync(Guid actorId)
        {
            var all = await _repo.GetRatingsByActorAsync(actorId);
            return all.Any()
                ? all.Average(r => r.Score)
                : 0.0;
        }

        public async Task UpsertRatingAsync(Guid actorId, Guid userId, int score)
        {
            var existing = await _repo.GetUserRatingAsync(actorId, userId);
            if (existing != null)
            {
                existing.Score = score;
                existing.CreatedAt = DateTime.UtcNow;
                _repo.Update(existing);
            }
            else
            {
                var r = new ActorRating
                {
                    ActorId = actorId,
                    UserId = userId,
                    Score = score
                };
                await _repo.AddAsync(r);
            }
            await _repo.SaveChangesAsync();
        }
    }
}
