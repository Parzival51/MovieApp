using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.UserDtos
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public int FavoritesCount { get; set; }
        public int WatchlistCount { get; set; }
        public int ReviewsCount { get; set; }   
        public int RatingsCount { get; set; }   
    }
}
