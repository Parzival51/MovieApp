using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Abstract
{
    public interface IActorRepository : IGenericRepository<Actor>
    {
        Task<Actor?> GetByExternalIdAsync(int externalId);

        Task<IEnumerable<Movie>> GetMoviesByActorAsync(Guid actorId);
    }
}
