using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.ReviewDtos
{
    public class UpdateReviewDto
    {
        [Required]
        public Guid Id { get; set; }

        

        [Required]
        public string Content { get; set; }

        [Range(1, 5)]
        public int Stars { get; set; }

        public bool IsApproved { get; set; }
    }
}
