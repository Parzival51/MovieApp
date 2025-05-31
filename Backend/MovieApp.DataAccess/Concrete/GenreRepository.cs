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
    public class GenreRepository : GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(MovieAppDbContext ctx) : base(ctx) { }

        public async Task<Genre> GetByExternalIdAsync(int externalId) =>
            await _context.Set<Genre>()
                .FirstOrDefaultAsync(g => g.ExternalId == externalId);
    }
}
