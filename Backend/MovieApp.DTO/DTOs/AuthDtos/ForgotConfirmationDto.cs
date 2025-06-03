using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.AuthDtos
{
    public class ForgotConfirmationDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
