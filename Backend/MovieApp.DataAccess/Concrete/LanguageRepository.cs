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
    public class LanguageRepository : GenericRepository<Language>, ILanguageRepository
    {
        private readonly MovieAppDbContext _ctx;

        public LanguageRepository(MovieAppDbContext ctx)
            : base(ctx)
        {
            _ctx = ctx;
        }

        public Task<Language?> GetByIso639Async(string iso639) =>
             _ctx.Languages
           .FirstOrDefaultAsync(l => l.Iso639 == iso639);
    }
}
