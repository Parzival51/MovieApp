using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class MovieLanguage
    {
        [Required]
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }

        [Required, MaxLength(2)]
        public string Iso639 { get; set; }
        public Language Language { get; set; }
    }
}
