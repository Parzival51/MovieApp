using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieApp.DTO.DTOs.MovieDtos
{
    public class CreateMovieDto
    {
        
        [Required]
        public int ExternalId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public int Duration { get; set; }  

        [Required, MaxLength(50)]
        public string Language { get; set; }

        [Required, MaxLength(100)]
        public string Country { get; set; }

        [Url]
        public string PosterUrl { get; set; }
        [Url]
        public string TrailerUrl { get; set; }


        public string Tagline { get; set; }
        public string Status { get; set; }
        public long Budget { get; set; }
        public long Revenue { get; set; }
        public string ImdbId { get; set; }
        [Url]
        public string HomepageUrl { get; set; }
        public bool IsAdult { get; set; }
        public string BackdropPath { get; set; }


        public IList<Guid> GenreIds { get; set; } = new List<Guid>();
        public IList<Guid> ActorIds { get; set; } = new List<Guid>();
        public IList<Guid> DirectorIds { get; set; } = new List<Guid>();

        public IList<string> SpokenLanguageCodes { get; set; } = new List<string>();

        public IList<ImageMetaDto> ImageMetas { get; set; } = new List<ImageMetaDto>();

        public IList<ActorCastDto> Cast { get; set; } = new List<ActorCastDto>();


    }
}
