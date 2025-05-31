using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class Rating : IUserOwnedEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        [Range(1, 10, ErrorMessage = "Puan 1 ile 10 arasında olmalıdır.")]
        public byte Score10 { get; set; }

        public DateTime RatedAt { get; set; } = DateTime.UtcNow;
    }
}
