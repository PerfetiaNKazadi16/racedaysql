using Microsoft.AspNetCore.Mvc;
using RaceDay.Models1;
using RaceDay.Service;

namespace RaceDay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Unauthorized();

            var profile = await _userService.GetProfileAsync(userId.Value);

            if (profile == null)
                return NotFound();

            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(Users updated)
        {
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return Unauthorized();

            if (!await _userService.UpdateProfileAsync(userId.Value, updated))
                return NotFound();

            return NoContent();
        }
    }
}