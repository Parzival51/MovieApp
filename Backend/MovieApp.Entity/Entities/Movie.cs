using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class Movie
    {
        public Movie()
        {
            MovieGenres = new List<MovieGenre>();
            MovieActors = new List<MovieActor>();
            MovieDirectors = new List<MovieDirector>();
            MovieLanguages = new List<MovieLanguage>();
            Reviews = new List<Review>();
            Ratings = new List<Rating>();
            Favorites = new List<Favorite>();
            Watchlists = new List<Watchlist>();
            Images = new List<MovieImage>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public int ExternalId { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? TrailerUrl { get; set; }

        public int Duration { get; set; }

        [Required, MaxLength(50)]
        public string Language { get; set; }

        [MaxLength(100)]
        public string? Country { get; set; }

        [Url, MaxLength(300)]
        public string? PosterUrl { get; set; }

        [MaxLength(200)]
        public string? BackdropPath { get; set; }


        [MaxLength(255)]
        public string? Tagline { get; set; }

        [MaxLength(30)]
        public string? Status { get; set; }

        public long Budget { get; set; }

        public long Revenue { get; set; }

        [MaxLength(15)]
        public string? ImdbId { get; set; }

        [Url, MaxLength(300)]
        public string? HomepageUrl { get; set; }

        public bool IsAdult { get; set; }


        public ICollection<MovieGenre> MovieGenres { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }
        public ICollection<MovieDirector> MovieDirectors { get; set; }
        public ICollection<MovieLanguage> MovieLanguages { get; set; }
        public ICollection<MovieImage> Images { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Watchlist> Watchlists { get; set; }
    }
}
