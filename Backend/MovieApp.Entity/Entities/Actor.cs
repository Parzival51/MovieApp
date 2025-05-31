using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieApp.Entity.Entities
{
    public class Actor
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = default!;

        public int ExternalId { get; set; }

        public byte Gender { get; set; }

        [MaxLength(200)]
        public string? ProfilePath { get; set; }    

     
        public float Popularity { get; set; }

       
        public string? Biography { get; set; }     

        public DateTime? Birthday { get; set; }   

        public string? PlaceOfBirth { get; set; }  

        public List<MovieActor> MovieActors { get; set; } = new List<MovieActor>();


    }
}
