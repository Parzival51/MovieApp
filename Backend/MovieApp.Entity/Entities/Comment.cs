using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class Comment : IUserOwnedEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ReviewId { get; set; }
        public Review Review { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
        public bool IsApproved { get; set; } = false;
    }
}
