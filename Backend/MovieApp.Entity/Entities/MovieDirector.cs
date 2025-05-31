using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class MovieDirector
    {
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }

        public Guid DirectorId { get; set; }
        public Director Director { get; set; }
    }
}
