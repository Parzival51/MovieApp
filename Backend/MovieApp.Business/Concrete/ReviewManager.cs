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
    public class ReviewManager : GenericManager<Review>, IReviewService
    {
        private readonly IReviewRepository _repo;
        public ReviewManager(IReviewRepository repo,
                             IHttpContextAccessor accessor)
            : base(repo, accessor) => _repo = repo;

        public Task<IEnumerable<Review>> GetPendingReviewsAsync() => _repo.GetPendingReviewsAsync();
        public Task<IEnumerable<Review>> GetReviewsByMovieAsync(Guid m) => _repo.GetReviewsByMovieAsync(m);
        public Task<Review> GetByIdWithDetailsAsync(Guid i) => _repo.GetByIdWithDetailsAsync(i);
    }

}
