using GeoBlocker.BLL.DTOs;
using GeoBlocker.BLL.Services.Abstraction;
using GeoBlocker.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeoBlocker.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService logService;

        public LogController(ILogService logService)
        {
            this.logService = logService;
        }
        [HttpGet("All-Activities")]
        [ProducesResponseType(typeof(PagedResult<string>), StatusCodes.Status200OK)]
        public IActionResult GetAllActivities([FromQuery] int page = 1,[FromQuery] int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize > 100 ? 100 : pageSize;

            var res = logService.GetAllActivityLogs(page , pageSize);

            return Ok(res);
        }

        [HttpGet("blocked-attempts")]
        [ProducesResponseType(typeof(PagedResult<BlockedAttemptLog>), StatusCodes.Status200OK)]
        public IActionResult GetAllBlockAttemps([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize > 100 ? 100 : pageSize;

            var res = logService.GetAllBlockedAttemptLogs(page, pageSize);

            return Ok(res);
        }

    }
}
