using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.DirectorDtos
{
    public class DirectorListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProfilePath { get; set; }
        public float Popularity { get; set; }
    }

}
