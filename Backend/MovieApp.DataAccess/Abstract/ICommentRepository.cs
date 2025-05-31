using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Abstract
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<Comment> GetByIdWithDetailsAsync(Guid id);
    }
}
