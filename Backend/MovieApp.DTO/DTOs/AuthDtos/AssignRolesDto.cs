using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.AuthDtos
{
    public class AssignRolesDto
    {
        [Required]
        public Guid UserId { get; set; }

        public IList<string> Roles { get; set; } = new List<string>();
    }
}
