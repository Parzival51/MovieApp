using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.UserDtos
{
    public class CreateUserDto
    {
        [Required, MaxLength(100)]
        public string UserName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
