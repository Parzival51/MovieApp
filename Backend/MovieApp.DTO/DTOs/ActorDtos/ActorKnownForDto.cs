using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.ActorDtos
{
    public class ActorKnownForDto
    {
        public Guid Id { get; set; }        
        public Guid MovieId { get; set; }
        public string Title { get; set; }
        public string Character { get; set; }
        public string PosterPath { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public float VoteAverage { get; set; }
    }
}
