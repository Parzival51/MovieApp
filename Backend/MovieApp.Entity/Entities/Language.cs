using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class Language
    {
        [Key, MaxLength(2)]
        public string Iso639 { get; set; }

        [Required, MaxLength(80)]
        public string EnglishName { get; set; }

        public ICollection<MovieLanguage> MovieLanguages { get; set; } = new List<MovieLanguage>();
    }
}
