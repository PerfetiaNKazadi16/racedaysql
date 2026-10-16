using Microsoft.AspNetCore.Mvc;
using RaceDay.Models1;
using RaceDay.Service;

namespace RaceDay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RouteController : ControllerBase
    {
        private readonly RouteService _routeService;

        public RouteController(RouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet("event/{eventId:int}")]
        public async Task<IActionResult> GetForEvent(int eventId)
        {
            return Ok(await _routeService.GetForEventAsync(eventId));
        }

        [HttpPost]
        public async Task<IActionResult> Create(route newRoute)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Unauthorized();

            var created = await _routeService.CreateAsync(userId.Value, newRoute);

            if (created == null)
                return NotFound("Event not found or it does not belong to this Organiser.");

            return Ok(created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, route updated)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Unauthorized();

            if (!await _routeService.UpdateAsync(userId.Value, id, updated))
                return NotFound("Route not found or it does not belong to this Organiser.");

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return Unauthorized();

            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Unauthorized();

            if (!await _routeService.DeleteAsync(userId.Value, id))
                return NotFound("Route not found or it does not belong to this Organiser.");

            return NoContent();
        }
    }
}