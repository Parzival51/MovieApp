using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.ExternalDtos;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class ExternalMovieService : IExternalMovieService
    {
        private readonly HttpClient _http;

        public ExternalMovieService(HttpClient http)
        {
            _http = http;
        }

        public async Task<TmdbFullMovieDto> GetFullDetailsAsync(string apiKey, int tmdbId)
        {
            var url =
                "https://api.themoviedb.org/3"             
                + $"/movie/{tmdbId}"                         
                + $"?api_key={apiKey}"                       
                + "&language=en-US"
                + "&append_to_response=credits,videos,images";

            var dto = await _http.GetFromJsonAsync<TmdbFullMovieDto>(url)
                      ?? throw new HttpRequestException($"No data returned for TMDb ID {tmdbId}");
            return dto;
        }
    }
}
