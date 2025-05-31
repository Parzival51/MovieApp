using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.UserDtos
{
    public class UserDetailDto : UserListDto
    {
        public DateTime? LastLogin { get; set; }
     
    }
}
