using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class MovieImage
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }
        public string Type { get; set; }
        public string FilePath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public float VoteAverage { get; set; }
    }
}
