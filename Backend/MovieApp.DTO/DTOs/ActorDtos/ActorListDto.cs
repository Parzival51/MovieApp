using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.ActorDtos
{
    public class ActorListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Biography { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PlaceOfBirth { get; set; }


    }
}
   