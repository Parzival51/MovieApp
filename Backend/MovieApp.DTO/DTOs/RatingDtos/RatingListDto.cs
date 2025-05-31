using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.RatingDtos
{
    public class RatingListDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public string UserName { get; set; }
        public byte Score10 { get; set; }
        public DateTime RatedAt { get; set; }
    }
}
