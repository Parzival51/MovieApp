using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Abstract
{
    public interface ILanguageRepository : IGenericRepository<Language>
    {

        Task<Language?> GetByIso639Async(string iso639);
    }
}

