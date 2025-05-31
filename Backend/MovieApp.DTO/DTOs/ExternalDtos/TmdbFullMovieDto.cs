using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.ExternalDtos
{
    public class TmdbFullMovieDto
    {
        public int id { get; set; }
        public bool adult { get; set; }
        public string backdrop_path { get; set; }
        public long budget { get; set; }
        public string homepage { get; set; }
        public string imdb_id { get; set; }
        public string original_language { get; set; }
        public string original_title { get; set; }
        public string overview { get; set; }
        public double popularity { get; set; }
        public string poster_path { get; set; }
        public string release_date { get; set; }
        public long revenue { get; set; }
        public int runtime { get; set; }
        public string status { get; set; }
        public string tagline { get; set; }
        public string title { get; set; }
        public bool video { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }

        public List<TmdbGenreDto> genres { get; set; }
        public List<TmdbProductionCountryDto> production_countries { get; set; }

        public TmdbCreditsDto credits { get; set; }
        public TmdbVideosDto videos { get; set; }
        public TmdbImagesDto images { get; set; }

        public List<TmdbLanguageDto> spoken_languages { get; set; }
    }

    public class TmdbProductionCountryDto
    {
        public string iso_3166_1 { get; set; }
        public string name { get; set; }
    }

    public class TmdbGenreDto
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class TmdbCreditsDto
    {
        public List<TmdbCastDto> cast { get; set; }
        public List<TmdbCrewDto> crew { get; set; }
    }

    public class TmdbCastDto
    {
        public bool adult { get; set; }
        public int gender { get; set; }
        public int id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public float popularity { get; set; }
        public string profile_path { get; set; }
        public int cast_id { get; set; }
        public string character { get; set; }
        public string credit_id { get; set; }
        public int order { get; set; }
    }

    public class TmdbCrewDto
    {
        public bool adult { get; set; }
        public int gender { get; set; }
        public int id { get; set; }
        public string known_for_department { get; set; }
        public string name { get; set; }
        public string original_name { get; set; }
        public float popularity { get; set; }
        public string profile_path { get; set; }
        public string department { get; set; }
        public string job { get; set; }
        public string credit_id { get; set; }
    }

    public class TmdbVideosDto
    {
        public List<TmdbVideoDto> results { get; set; }
    }

    public class TmdbVideoDto
    {
        public string id { get; set; }
        public string iso_639_1 { get; set; }
        public string iso_3166_1 { get; set; }
        public string key { get; set; }
        public string name { get; set; }
        public string site { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public bool official { get; set; }
        public string published_at { get; set; }
    }

    public class TmdbImagesDto
    {
        public List<TmdbImageDto> backdrops { get; set; }
        public List<TmdbImageDto> posters { get; set; }
        public List<TmdbImageDto> logos { get; set; }
    }

    public class TmdbImageDto
    {
        public string file_path { get; set; }
        public double vote_average { get; set; }
        public int vote_count { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string iso_639_1 { get; set; }
    }
}
