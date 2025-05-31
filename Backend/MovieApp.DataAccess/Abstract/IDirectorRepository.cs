using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Abstract
{
    public interface IDirectorRepository : IGenericRepository<Director>
    {
        Task<Director?> GetByExternalIdAsync(int externalId);

        Task<IEnumerable<Movie>> GetMoviesByDirectorAsync(Guid directorId);

    }
}
