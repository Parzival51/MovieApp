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
    public class DirectorRatingManager : IDirectorRatingService
    {
        private readonly IDirectorRatingRepository _repo;

        public DirectorRatingManager(IDirectorRatingRepository repo)
        {
            _repo = repo;
        }

        public Task<DirectorRating?> GetUserRatingAsync(Guid directorId, Guid userId) =>
            _repo.GetUserRatingAsync(directorId, userId);

        public Task<IEnumerable<DirectorRating>> GetRatingsByDirectorAsync(Guid directorId) =>
            _repo.GetRatingsByDirectorAsync(directorId);

        public async Task<double> GetAverageRatingAsync(Guid directorId)
        {
            var all = await _repo.GetRatingsByDirectorAsync(directorId);
            return all.Any() ? all.Average(r => r.Score) : 0.0;
        }

        public async Task UpsertRatingAsync(Guid directorId, Guid userId, int score)
        {
            var existing = await _repo.GetUserRatingAsync(directorId, userId);
            if (existing != null)
            {
                existing.Score = score;
                existing.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                var r = new DirectorRating
                {
                    DirectorId = directorId,
                    UserId = userId,
                    Score = score
                };
                await (_repo as dynamic).AddAsync(r);
            }
            await (_repo as dynamic).SaveChangesAsync();
        }
    }
}
