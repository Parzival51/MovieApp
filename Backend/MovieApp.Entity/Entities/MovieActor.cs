using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class MovieActor
    {
        [Required]
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }

        [Required]
        public Guid ActorId { get; set; }
        public Actor Actor { get; set; }

        public short Order { get; set; }

        public string Character { get; set; }
    }
}
