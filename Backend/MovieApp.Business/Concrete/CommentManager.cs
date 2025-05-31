using Microsoft.AspNetCore.Http;
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
    public class CommentManager : GenericManager<Comment>, ICommentService
    {
        private readonly ICommentRepository _commentRepo;

        public CommentManager(
            ICommentRepository commentRepo,
            IHttpContextAccessor httpContextAccessor)    
            : base(commentRepo, httpContextAccessor)      
        {
            _commentRepo = commentRepo;
        }

        public Task<Comment> GetByIdWithDetailsAsync(Guid id) =>
            _commentRepo.GetByIdWithDetailsAsync(id);
    }
}
