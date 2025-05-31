using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.ReviewDtos
{
    public class ReviewDetailDto : ReviewListDto
    {
        public string Content { get; set; }
        public IList<string> Comments { get; set; }
    }
}
