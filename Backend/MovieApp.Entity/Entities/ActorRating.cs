using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class ActorRating
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ActorId { get; set; }
        public Actor Actor { get; set; }

        [Required]
        public Guid UserId { get; set; }
        public User User { get; set; }

        [Range(1, 10)]
        public int Score { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
