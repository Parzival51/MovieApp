using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IDirectorService : IGenericService<Director>
    {

        Task<Director?> GetByExternalIdAsync(int externalId);


        Task<Director> CreateFromExternalAsync(TmdbCrewDto dto);

        Task<IEnumerable<Movie>> GetMoviesByDirectorAsync(Guid directorId);

    }
}
