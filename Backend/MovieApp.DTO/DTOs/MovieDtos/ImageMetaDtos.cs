using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.MovieDtos
{
    public class ImageMetaDto
    {
        public string Type { get; set; }
        public string FilePath { get; set; }
        public short Width { get; set; }
        public short Height { get; set; }
        public float VoteAverage { get; set; }
    }
}
