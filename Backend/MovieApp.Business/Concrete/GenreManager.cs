using Microsoft.AspNetCore.Http;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Abstract;
using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class GenreManager : GenericManager<Genre>, IGenreService
    {
        private readonly IGenreRepository _repo;

        public GenreManager(IGenreRepository repo, IHttpContextAccessor httpCtx)
           : base(repo, httpCtx)
        {
            _repo = repo;
        }

        public Task<Genre> GetByExternalIdAsync(int externalId) =>
            _repo.GetByExternalIdAsync(externalId);

        public async Task<Genre> CreateFromExternalAsync(TmdbGenreDto external)
        {
            var entity = new Genre
            {
                ExternalId = external.id,
                Name = external.name
            };
            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }
    }
}
