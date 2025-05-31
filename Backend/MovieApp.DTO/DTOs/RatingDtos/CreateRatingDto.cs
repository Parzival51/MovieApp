using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.RatingDtos
{
    public class CreateRatingDto
    {
        [Required] public Guid MovieId { get; set; }

        [Range(1, 10)] public byte Score10 { get; set; }
    }
}
