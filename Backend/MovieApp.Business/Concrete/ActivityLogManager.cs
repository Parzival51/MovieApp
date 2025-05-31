using AutoMapper;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Abstract;
using MovieApp.DTO.DTOs.ActivityLogDtos;
using MovieApp.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp.Business.Concrete
{
    public class ActivityLogManager : IActivityLogService
    {
        private readonly IActivityLogRepository _repo;
        private readonly IMapper _mapper;

        public ActivityLogManager(IActivityLogRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<IEnumerable<ActivityLogListDto>> GetPagedAsync(
            Guid? userId,
            DateTime? from,
            DateTime? to,
            int page,
            int pageSize)
        {
            var entities = await _repo.GetPagedAsync(userId, from, to, page, pageSize);
            return _mapper.Map<IEnumerable<ActivityLogListDto>>(entities);
        }

        public Task<ActivityLog> GetByIdAsync(Guid id)
            => _repo.GetByIdAsync(id);                      

        public Task<IEnumerable<ActivityLog>> GetFilteredListAsync(
                Expression<Func<ActivityLog, bool>> predicate)
            => _repo.GetFilteredListAsync(predicate);        
    }
}
