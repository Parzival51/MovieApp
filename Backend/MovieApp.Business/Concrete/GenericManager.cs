using Microsoft.AspNetCore.Http;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Abstract;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class
    {
        protected readonly IGenericRepository<T> _repository;
        private readonly IHttpContextAccessor _httpContext;

        public GenericManager(
            IGenericRepository<T> repository,
            IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContext = httpContextAccessor;
        }

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<T> GetByIdAsync(params object[] keyValues) =>
            await _repository.GetByIdAsync(keyValues);

        public async Task<IEnumerable<T>> GetFilteredListAsync(Expression<Func<T, bool>> predicate) =>
            await _repository.GetFilteredListAsync(predicate);

        public async Task<T> CreateAsync(T entity)
        {
            if (entity is IUserOwnedEntity owned)
            {
                var userIdStr = _httpContext.HttpContext?
                    .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdStr))
                    throw new InvalidOperationException("Token'dan UserId alınamadı.");

                owned.UserId = Guid.Parse(userIdStr);
            }

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _repository.Update(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _repository.Delete(entity);
            await _repository.SaveChangesAsync();
        }
    }
}
