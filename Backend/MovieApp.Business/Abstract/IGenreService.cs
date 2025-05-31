using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IGenreService : IGenericService<Genre>
    {
      
        Task<Genre> GetByExternalIdAsync(int externalId);
        Task<Genre> CreateFromExternalAsync(TmdbGenreDto external);
    }
}
