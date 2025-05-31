using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DataAccess.Abstract
{
    public interface ILanguageService : IGenericService<Language>
    {

        Task<Language?> GetByIso639Async(string iso639);

        Task<Language> CreateFromExternalAsync(TmdbLanguageDto dto);
    }
}
