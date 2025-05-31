using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IReviewService : IGenericService<Review>
    {
        Task<IEnumerable<Review>> GetPendingReviewsAsync();
        Task<IEnumerable<Review>> GetReviewsByMovieAsync(Guid movieId);

        Task<Review> GetByIdWithDetailsAsync(Guid id);


    }
}
