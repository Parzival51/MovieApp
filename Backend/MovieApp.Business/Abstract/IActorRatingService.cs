using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IActorRatingService
    {
        Task<ActorRating> GetUserRatingAsync(Guid actorId, Guid userId);
        Task<IEnumerable<ActorRating>> GetRatingsByActorAsync(Guid actorId);
        Task<double> GetAverageRatingAsync(Guid actorId);
        Task UpsertRatingAsync(Guid actorId, Guid userId, int score);
    }
}
