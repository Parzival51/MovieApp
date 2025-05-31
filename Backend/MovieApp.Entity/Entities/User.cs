using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class User : IdentityUser<Guid>
    {
        [Required, MaxLength(100)]
        public string DisplayName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Watchlist> Watchlists { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<ActivityLog> ActivityLogs { get; set; }
    }
}
