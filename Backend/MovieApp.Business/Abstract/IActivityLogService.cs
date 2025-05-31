using MovieApp.DTO.DTOs.ActivityLogDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Abstract
{
    public interface IActivityLogService
    {
        Task<IEnumerable<ActivityLogListDto>> GetPagedAsync(
            Guid? userId,
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize);

        Task<ActivityLog> GetByIdAsync(Guid id);

        Task<IEnumerable<ActivityLog>> GetFilteredListAsync(
            Expression<Func<ActivityLog, bool>> predicate);
    }
}
