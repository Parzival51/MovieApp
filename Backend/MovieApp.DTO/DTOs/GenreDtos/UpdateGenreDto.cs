using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.GenreDtos
{
    public class UpdateGenreDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }
    }
}
