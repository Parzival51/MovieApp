using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.CommentDtos
{
    public class UpdateCommentDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required, MaxLength(1000)]
        public string Content { get; set; }
    }
}
