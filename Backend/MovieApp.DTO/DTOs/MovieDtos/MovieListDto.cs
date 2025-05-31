    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace MovieApp.DTO.DTOs.MovieDtos
    {
    public class MovieListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; }
        public double AverageRating { get; set; }

        public int Duration { get; set; }
        public string Language { get; set; }
        public string Country { get; set; }
    }
}
