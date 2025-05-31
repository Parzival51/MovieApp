using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IRatingService : IGenericService<Rating>
    {
        Task<Rating> AddOrUpdateAsync(Rating rating);
    }
}
