using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.DirectorDtos
{
    public class CreateDirectorDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }

        public DateTime? BirthDate { get; set; }
        public string Bio { get; set; }
    }
}
