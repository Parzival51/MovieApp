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
    public class FavoriteManager : GenericManager<Favorite>, IFavoriteService
    {
        public FavoriteManager(IGenericRepository<Favorite> repo,
                               IHttpContextAccessor accessor)
            : base(repo, accessor) { }
    }
}
