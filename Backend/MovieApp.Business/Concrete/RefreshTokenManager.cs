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
    public class RefreshTokenManager : GenericManager<RefreshToken>, IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _tokenRepository;

        public RefreshTokenManager(
            IRefreshTokenRepository tokenRepository,
            IHttpContextAccessor httpContextAccessor) 
            : base(tokenRepository, httpContextAccessor) 
        {
            _tokenRepository = tokenRepository;
        }


        public Task<RefreshToken> GetByTokenAsync(string token) =>
            _tokenRepository.GetByTokenAsync(token);
    }
}
