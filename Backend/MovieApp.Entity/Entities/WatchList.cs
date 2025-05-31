using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{

    public class Watchlist : IUserOwnedEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
