using Microsoft.AspNetCore.Mvc;
using RaceDay.Models1;
using RaceDay.Service;

namespace RaceDay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EntryController : ControllerBase
    {
        private readonly EntryService _entryService;

        public EntryController(EntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpPost]
        public async Task<IActionResult> Enrol(Entry entry)
        {
            if (HttpContext.Session.GetString("Role") != "Participant")
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Unauthorized();

            var created = await _entryService.EnrolAsync(userId.Value, entry);

            if (created == null)
                return BadRequest(
                    "Participant not found, category does not belong to the event, or participant is already entered.");

            return Ok(created);
        }

        [HttpGet("my-events")]
        public async Task<IActionResult> GetForOrganiser()
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Unauthorized();

            return Ok(await _entryService.GetForOrganiserAsync(userId.Value));
        }
    }
}
