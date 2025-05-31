
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieApp.Business.Abstract;
using MovieApp.DTO.DTOs.ActivityLogDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ActivityLogsController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;
        private readonly IMapper _mapper;

        public ActivityLogsController(IActivityLogService activityLogService,
                                      IMapper mapper)
        {
            _activityLogService = activityLogService;
            _mapper = mapper;
        }

        // GET: api/activitylogs
        // GET: api/activitylogs
        [HttpGet]
        public async Task<IActionResult> GetPaged(
                [FromQuery] Guid? userId = null,
                [FromQuery] DateTime? from = null,
                [FromQuery] DateTime? to = null,
                [FromQuery] int page = 1,
                [FromQuery] int pageSize = 100)
        {
            var list = await _activityLogService.GetPagedAsync(
                            userId, from, to, page, pageSize);
            return Ok(list);
        }


        // GET: api/activitylogs/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var log = await _activityLogService.GetByIdAsync(id);
            if (log == null)
                return NotFound();

            var dto = _mapper.Map<ActivityLogListDto>(log);
            return Ok(dto);
        }

        // GET: api/activitylogs/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUser(Guid userId)
        {
            var logs = await _activityLogService
                .GetFilteredListAsync(al => al.UserId == userId);
            var dtos = _mapper.Map<IEnumerable<ActivityLogListDto>>(logs);
            return Ok(dtos);
        }
    }
}
