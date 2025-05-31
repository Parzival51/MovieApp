using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IDirectorRatingService
    {
        Task<DirectorRating?> GetUserRatingAsync(Guid directorId, Guid userId);
        Task<IEnumerable<DirectorRating>> GetRatingsByDirectorAsync(Guid directorId);
        Task<double> GetAverageRatingAsync(Guid directorId);
        Task UpsertRatingAsync(Guid directorId, Guid userId, int score);
    }
}
