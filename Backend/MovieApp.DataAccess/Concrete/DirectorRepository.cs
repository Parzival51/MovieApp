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
    public class DirectorRepository : GenericRepository<Director>, IDirectorRepository
    {
        private readonly MovieAppDbContext _ctx;

        public DirectorRepository(MovieAppDbContext ctx)
            : base(ctx)
        {
            _ctx = ctx;
        }

        public Task<Director?> GetByExternalIdAsync(int externalId) =>
            _ctx.Directors
                .FirstOrDefaultAsync(d => d.ExternalId == externalId);


        public async Task<IEnumerable<Movie>> GetMoviesByDirectorAsync(Guid directorId)
        {
            return await _ctx.Movies
                .Include(m => m.MovieDirectors)
                .Include(m => m.Ratings)      
                .Where(m => m.MovieDirectors.Any(md => md.DirectorId == directorId))
                .ToListAsync();
        }
    }
}
