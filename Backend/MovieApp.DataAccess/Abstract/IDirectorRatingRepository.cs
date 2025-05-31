using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Abstract
{
    public interface IDirectorRatingRepository : IGenericRepository<DirectorRating>
    {
        Task<DirectorRating?> GetUserRatingAsync(Guid directorId, Guid userId);
        Task<IEnumerable<DirectorRating>> GetRatingsByDirectorAsync(Guid directorId);
    }
}
