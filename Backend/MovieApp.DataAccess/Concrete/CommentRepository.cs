using Microsoft.EntityFrameworkCore;
using MovieApp.DataAccess.Abstract;
using MovieApp.DataAccess.Context;
using MovieApp.DataAccess.Repositories;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Concrete
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly MovieAppDbContext _context;

        public CommentRepository(MovieAppDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<IEnumerable<Comment>> GetFilteredListAsync(
    Expression<Func<Comment, bool>> predicate)
        {
            return await _context.Comments
                                 .Include(c => c.User)
                                 .Where(predicate)
                                 .ToListAsync();
        }

        public async Task<Comment> GetByIdWithDetailsAsync(Guid id) =>
            await _context.Comments
                          .Include(c => c.User)
                          .Include(c => c.Review).ThenInclude(r => r.User)
                          .FirstOrDefaultAsync(c => c.Id == id);
    }

}
