using Microsoft.AspNetCore.Mvc;
using TimeTracker.BLL.Services;
using TimeTracker.DTO;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _service;

        public ActivityController(IActivityService service)
        {
            _service = service;
        }

        // POST api/activity/report
        [HttpPost("report")]
        public async Task<IActionResult> Report(ActivityReportDto report)
        {
            try
            {
                await _service.ReportActivityAsync(report);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}