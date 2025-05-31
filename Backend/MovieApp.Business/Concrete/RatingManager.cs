using Microsoft.AspNetCore.Http;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Abstract;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class RatingManager : GenericManager<Rating>, IRatingService
    {
        private readonly IRatingRepository _repo;
        private readonly IHttpContextAccessor _ctx;        
        public RatingManager(IRatingRepository repo,
                             IHttpContextAccessor ctx)
            : base(repo, ctx)
        {
            _repo = repo;
            _ctx = ctx;                                   
        }


        public async Task<Rating> AddOrUpdateAsync(Rating r)
        {
            if (r.UserId == Guid.Empty)
            {
                var uidStr = _ctx.HttpContext?
                             .User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?
                             .Value;

                if (string.IsNullOrEmpty(uidStr))
                    throw new UnauthorizedAccessException("Token yok veya geçersiz");

                r.UserId = Guid.Parse(uidStr);
            }

            var existing = await _repo.GetByMovieAndUserAsync(r.MovieId, r.UserId);

            if (existing is null)
            {
                await _repo.AddAsync(r);
                await _repo.SaveChangesAsync();
                return r;
            }

            existing.Score10 = r.Score10;
            existing.RatedAt = DateTime.UtcNow;

            _repo.Update(existing);
            await _repo.SaveChangesAsync();
            return existing;
        }
    }
}
