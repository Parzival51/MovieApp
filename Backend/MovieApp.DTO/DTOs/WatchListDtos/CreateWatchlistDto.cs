using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.DTO.DTOs.WatchListDtos
{
    public class CreateWatchlistDto
    {
        [Required]
        public Guid MovieId { get; set; }
    }
}
