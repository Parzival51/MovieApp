using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Abstract
{
    public interface IActorRatingRepository : IGenericRepository<ActorRating>
    {
        Task<ActorRating> GetUserRatingAsync(Guid actorId, Guid userId);
        Task<IEnumerable<ActorRating>> GetRatingsByActorAsync(Guid actorId);
    }
}
