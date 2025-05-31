using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.RoleDtos
{
    public class UpdateRoleDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
