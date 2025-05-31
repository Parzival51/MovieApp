using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.ActorDtos
{
    public class ActorDetailDto : ActorListDto
    {
        public int ExternalId { get; set; }
        public float Popularity { get; set; }
        public IList<string> AlsoKnownAs { get; set; } = new List<string>();
        public IList<ActorKnownForDto> KnownFor { get; set; } = new List<ActorKnownForDto>();
    }
}
