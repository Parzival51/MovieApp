using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IRefreshTokenService : IGenericService<RefreshToken>
    {
        Task<RefreshToken> GetByTokenAsync(string token);
    }
}
