using Microsoft.AspNetCore.Mvc;
using TimeTracker.BLL.Services;
using TimeTracker.DTO;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        // GET api/reports/daily
        // GET api/reports/daily?date=2026-11-17
        [HttpGet("daily")]
        public async Task<ActionResult<DailyReportDto>> GetDaily([FromQuery] DateTime? date)
        {
            var report = await _service.GetDailyReportAsync(date ?? DateTime.Today);
            return Ok(report);
        }
    }
}