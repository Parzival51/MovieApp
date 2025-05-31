using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class Director
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public byte Gender { get; set; }

        public int ExternalId { get; set; }

        [MaxLength(200)]
        public string? ProfilePath { get; set; }

        public float Popularity { get; set; }


        public string Biography { get; set; }


        public DateTime? Birthday { get; set; }


        public string PlaceOfBirth { get; set; }

        public ICollection<MovieDirector> MovieDirectors { get; set; } = new List<MovieDirector>();

        public ICollection<DirectorRating> Ratings { get; set; } = new List<DirectorRating>();



    }
}
