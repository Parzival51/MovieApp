using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IActorService : IGenericService<Actor>
    {

        Task<Actor?> GetByExternalIdAsync(int externalId);


        Task<Actor> CreateFromExternalAsync(TmdbCastDto dto);


        Task<IEnumerable<Movie>> GetMoviesByActorAsync(Guid actorId);
    }
}
