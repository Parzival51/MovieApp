using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Entity.Entities
{
    public interface IUserOwnedEntity
    {
        Guid UserId { get; set; }
    }
}
