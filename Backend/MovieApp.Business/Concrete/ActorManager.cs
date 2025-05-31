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
    public class ActorManager : GenericManager<Actor>, IActorService
    {
        private readonly IActorRepository _repo;

        public ActorManager(
            IActorRepository repo,
            IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
            _repo = repo;
        }

        public Task<Actor?> GetByExternalIdAsync(int externalId) =>
            _repo.GetByExternalIdAsync(externalId);

        public async Task<Actor> CreateFromExternalAsync(TmdbCastDto dto)
        {
            var entity = new Actor
            {
                ExternalId = dto.id,
                Name = dto.name,
                ProfilePath = dto.profile_path,
                Popularity = dto.popularity
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }

        public Task<IEnumerable<Movie>> GetMoviesByActorAsync(Guid actorId) =>
            _repo.GetMoviesByActorAsync(actorId);
    }
}
