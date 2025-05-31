using MovieApp.DTO.DTOs.ExternalDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IExternalPersonService
    {
        Task<TmdbPersonDto> GetPersonAsync(string apiKey, int personId);
        Task<TmdbImagesDto> GetPersonImagesAsync(string apiKey, int personId);
        Task<TmdbCreditsDto> GetPersonMovieCreditsAsync(string apiKey, int personId);
    }
}
