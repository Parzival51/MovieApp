using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.UserDtos
{
    public class UpdateUserDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public string NewPassword { get; set; }

        public IList<string> Roles { get; set; } = new List<string>();
    }
}
