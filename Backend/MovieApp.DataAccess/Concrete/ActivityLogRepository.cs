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
    public class ActivityLogRepository
       : GenericRepository<ActivityLog>, IActivityLogRepository
    {
        public ActivityLogRepository(MovieAppDbContext ctx) : base(ctx) { }

        public async Task<IEnumerable<ActivityLog>> GetPagedAsync(
            Guid? userId,
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize)
        {
            var q = _dbSet                             
                      .Include(l => l.User)            
                      .AsQueryable();

            if (userId.HasValue) q = q.Where(l => l.UserId == userId);
            if (from.HasValue) q = q.Where(l => l.Timestamp >= from);
            if (to.HasValue) q = q.Where(l => l.Timestamp <= to);

            return await q.OrderByDescending(l => l.Timestamp)
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();
        }
    }
}
