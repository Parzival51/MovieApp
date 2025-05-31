using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.DirectorDtos
{
    public class DirectorDetailDto : DirectorListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProfilePath { get; set; }
        public float Popularity { get; set; }

        public DateTime? Birthday { get; set; }
        public string PlaceOfBirth { get; set; }
        public IList<string> AlsoKnownAs { get; set; } = new List<string>();
        public string Biography { get; set; }
    }
}
