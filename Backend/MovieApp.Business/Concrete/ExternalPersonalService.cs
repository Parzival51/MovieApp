using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.ExternalDtos;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class ExternalPersonService : IExternalPersonService
    {
        private readonly HttpClient _http;

        public ExternalPersonService(HttpClient http)
        {
            _http = http;
        }


        public async Task<TmdbPersonDto> GetPersonAsync(string apiKey, int personId)
        {
            var url =
                "https://api.themoviedb.org/3"
                + $"/person/{personId}"
                + $"?api_key={apiKey}"
                + "&language=en-US";

            return await _http.GetFromJsonAsync<TmdbPersonDto>(url)
                   ?? throw new HttpRequestException($"No person found for TMDb ID {personId}");
        }


        public async Task<TmdbImagesDto> GetPersonImagesAsync(string apiKey, int personId)
        {
            var url =
                "https://api.themoviedb.org/3"
                + $"/person/{personId}/images"
                + $"?api_key={apiKey}";

            return await _http.GetFromJsonAsync<TmdbImagesDto>(url)
                   ?? throw new HttpRequestException($"No images found for TMDb person ID {personId}");
        }


        public async Task<TmdbCreditsDto> GetPersonMovieCreditsAsync(string apiKey, int personId)
        {
            var url =
                "https://api.themoviedb.org/3"
                + $"/person/{personId}/movie_credits"
                + $"?api_key={apiKey}"
                + "&language=en-US";

            return await _http.GetFromJsonAsync<TmdbCreditsDto>(url)
                   ?? throw new HttpRequestException($"No credits found for TMDb person ID {personId}");
        }
    }
}
