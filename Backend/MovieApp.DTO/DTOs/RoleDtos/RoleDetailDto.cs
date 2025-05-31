using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.RoleDtos
{
    public class RoleDetailDto : RoleListDto
    {
        public IList<string> AssignedUsers { get; set; }
    }
}
