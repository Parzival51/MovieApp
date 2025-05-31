using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.ExternalDtos
{
    public class TmdbPersonDto
    {
        public byte gender;

        public int id { get; set; }
        public string name { get; set; }
        public string biography { get; set; }
        public string birthday { get; set; }
        public string place_of_birth { get; set; }
        public string profile_path { get; set; }
        public double popularity { get; set; }
        public IList<string> also_known_as { get; set; } = new List<string>();
    }
}
