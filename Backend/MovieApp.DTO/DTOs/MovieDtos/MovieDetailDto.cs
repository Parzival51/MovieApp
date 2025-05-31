using MovieApp.DTO.DTOs.ActorDtos;
using MovieApp.DTO.DTOs.ExternalDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.MovieDtos
{
    public class MovieDetailDto : MovieListDto
    {
        public string Description { get; set; }
        public int Duration { get; set; }          
        public string Language { get; set; }
        public string Country { get; set; }

        public string Status { get; set; }
        public string Tagline { get; set; }
        public long Budget { get; set; }
        public long Revenue { get; set; }
        public string ImdbId { get; set; }
        public string HomepageUrl { get; set; }
        public bool IsAdult { get; set; }
        public string BackdropPath { get; set; }

        public IList<string> Genres { get; set; } = new List<string>();
        public IList<string> Directors { get; set; } = new List<string>();
        public IList<string> Actors { get; set; } = new List<string>();

        public IList<MovieLangDto> SpokenLanguages { get; set; } = new List<MovieLangDto>();
        public IList<MovieImageDto> Posters { get; set; } = new List<MovieImageDto>();
        public IList<MovieImageDto> Backdrops { get; set; } = new List<MovieImageDto>();

        public IList<ActorKnownForDto> TopCast { get; set; } = new List<ActorKnownForDto>();

        public IList<ActorCastDto> Cast { get; set; } = new List<ActorCastDto>();


    }
}
