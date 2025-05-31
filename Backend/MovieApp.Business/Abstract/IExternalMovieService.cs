using MovieApp.DTO.DTOs.ExternalDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IExternalMovieService
    {
    
        Task<TmdbFullMovieDto> GetFullDetailsAsync(string apiKey, int tmdbId);
    }
}
