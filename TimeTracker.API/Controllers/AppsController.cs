using Microsoft.AspNetCore.Mvc;
using TimeTracker.BLL.Services;
using TimeTracker.DTO;

namespace TimeTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppsController : ControllerBase
    {
        private readonly ITrackedAppService _service;

        public AppsController(ITrackedAppService service)
        {
            _service = service;
        }

        // GET api/apps
        [HttpGet]
        public async Task<ActionResult<List<TrackedAppDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        // PUT api/apps/5/category
        [HttpPut("{id}/category")]
        public async Task<IActionResult> AssignCategory(int id, AssignCategoryDto dto)
        {
            try
            {
                var updated = await _service.AssignCategoryAsync(id, dto.CategoryId);
                if (!updated)
                    return NotFound();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}