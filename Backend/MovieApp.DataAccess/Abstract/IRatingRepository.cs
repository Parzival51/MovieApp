using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Abstract
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<Rating?> GetByMovieAndUserAsync(Guid movieId, Guid userId);
        Task<IEnumerable<Rating>> GetByMovieAsync(Guid movieId);     // ★

        Task<IEnumerable<Rating>> GetAllAsync();
    }
}
