using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.MovieDtos
{
 
    public class ActorCastDto
    {
        
        public Guid ActorId { get; set; }

        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public string Character { get; set; }

        public short Order { get; set; }
    }
}
