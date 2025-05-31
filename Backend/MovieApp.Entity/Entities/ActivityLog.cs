using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public class ActivityLog
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public User User { get; set; }

        [Required]
        public string Action { get; set; }
        [Required]
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
