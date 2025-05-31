using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface ICommentService : IGenericService<Comment>
    {
        Task<Comment> GetByIdWithDetailsAsync(Guid id);
    }
}
