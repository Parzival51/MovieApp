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
    public class DirectorRatingRepository
       : GenericRepository<DirectorRating>, IDirectorRatingRepository
    {
        public DirectorRatingRepository(MovieAppDbContext context)
            : base(context)
        {
        }

        public async Task<DirectorRating?> GetUserRatingAsync(Guid directorId, Guid userId) =>
            await _dbSet
                .FirstOrDefaultAsync(r => r.DirectorId == directorId && r.UserId == userId);

        public async Task<IEnumerable<DirectorRating>> GetRatingsByDirectorAsync(Guid directorId) =>
            await _dbSet
                .Where(r => r.DirectorId == directorId)
                .ToListAsync();
    }
}
