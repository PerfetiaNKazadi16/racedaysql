using Microsoft.AspNetCore.Mvc;
using RaceDay.Models1;
using RaceDay.Service;
using RaceDay.Service.RaceDay.Service;

namespace RaceDay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventsService _eventService;

        public EventsController(EventsService eventService)
        {
            _eventService = eventService;
        }

        // Anyone can view events.
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var raceEvent = await _eventService.GetByIdAsync(id);

            if (raceEvent == null)
                return NotFound();

            return Ok(raceEvent);
        }

        // Only a logged-in Organiser can create, update, or delete events.
        [HttpPost]
        public async Task<IActionResult> Create(Events newEvent)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return Unauthorized();

            var createdEvent = await _eventService.CreateAsync(newEvent);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdEvent.EventsId },
                createdEvent);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Events updatedEvent)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return Unauthorized();

            var updated = await _eventService.UpdateAsync(id, updatedEvent);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Organiser")
                return Unauthorized();

            var deleted = await _eventService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}