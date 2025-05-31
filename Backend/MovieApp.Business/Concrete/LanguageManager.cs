using Microsoft.AspNetCore.Http;
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
    public class LanguageManager : GenericManager<Language>, ILanguageService
    {
        private readonly ILanguageRepository _repo;

        public LanguageManager(
            ILanguageRepository repo,
            IHttpContextAccessor httpContextAccessor)
            : base(repo, httpContextAccessor)
        {
            _repo = repo;
        }

        public Task<Language?> GetByIso639Async(string iso639) =>
            _repo.GetByIso639Async(iso639);


        public async Task<Language> CreateFromExternalAsync(TmdbLanguageDto dto)
        {
            var entity = new Language
            {
                Iso639 = dto.iso_639_1,
                EnglishName = dto.english_name
            };

            await _repo.AddAsync(entity);
            await _repo.SaveChangesAsync();
            return entity;
        }



    }
}
