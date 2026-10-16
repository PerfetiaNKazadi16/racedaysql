using Microsoft.AspNetCore.Mvc;
using RaceDay.Data1;
using RaceDay.Models1;
using RaceDay.Service;

namespace RaceDay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultController : ControllerBase
    {
        private readonly ResultService _resultService;

        public ResultController(ResultService resultService)
        {
            _resultService = resultService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(Result result)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Unauthorized();

            var saved = await _resultService.AddAsync(userId.Value, result);

            if (saved == null)
                return BadRequest(
                    "The event may not belong to you, the participant may not be entered, or a result already exists.");

            return Ok(saved);
        }

        [HttpGet("my-results")]
        public async Task<IActionResult> GetMyResults()
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Unauthorized();

            return Ok(await _resultService.GetForParticipantAsync(userId.Value));
        }
    }
}

 