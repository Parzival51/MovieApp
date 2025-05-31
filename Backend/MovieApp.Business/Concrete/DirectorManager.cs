using Microsoft.AspNetCore.Http;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Abstract;
using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.Entity.Entities;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class DirectorManager : GenericManager<Director>, IDirectorService
    {
        private readonly IDirectorRepository _repo;

        public DirectorManager(
            IDirectorRepository repo,
            IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
            _repo = repo;
        }

        public Task<Director?> GetByExternalIdAsync(int externalId) =>
            _repo.GetByExternalIdAsync(externalId);

        public async Task<Director> CreateFromExternalAsync(TmdbCrewDto dto)
        {
            var entity = new Director
            {
                ExternalId = dto.id,
                Name = dto.name,
                ProfilePath = dto.profile_path,
                Popularity = dto.popularity,

                Biography = string.Empty,
                PlaceOfBirth = string.Empty
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }

        public Task<IEnumerable<Movie>> GetMoviesByDirectorAsync(Guid directorId) =>
           _repo.GetMoviesByDirectorAsync(directorId);
    }
}
