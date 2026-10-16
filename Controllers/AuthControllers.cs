using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using RaceDay.Data1;
using RaceDay.Models1;
using RaceDay.DTOs1;
using Microsoft.EntityFrameworkCore;
using RegisterRequest = RaceDay.DTOs1.RegisterRequest;
using LoginRequest = RaceDay.DTOs1.LoginRequest;


namespace RaceDay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : SessionApiController
    {
        private readonly RaceDayContext _db;

        public AuthController(RaceDayContext db) => _db = db;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            string role = request.Role.Trim();

            if (role.Equals("organiser", StringComparison.OrdinalIgnoreCase))
                role = "Organiser";
            else if (role.Equals("participant", StringComparison.OrdinalIgnoreCase))
                role = "Participant";
            else
                return BadRequest("Role must be Organiser or Participant.");

            if (role == "Participant" && string.IsNullOrWhiteSpace(request.ParticipantName))
                return BadRequest("ParticipantName is required for a participant account.");

            if (await _db.Users.AnyAsync(u =>
                u.Email == request.Email || u.UserName == request.UserName))
                return Conflict("That email or username is already registered.");

            var user = new Users
            {
                Email = request.Email.Trim(),
                UserName = request.UserName.Trim(),
                role = role,
                AccountType = role == "Organiser" ? "Admin" : "Standard",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
            };

            if (role == "Organiser")
            {
                int nextNumber =
                  (await _db.Organisers.Select(o => (int?)o.OrganiserNumber).MaxAsync() ?? 1000) + 1;

                user.Organiser = new Organiser
                {
                    OrganiserNumber = request.OrganiserNumber ?? nextNumber,
                    Users = user
                };
            }
            else
            {
                user.Participant = new Participant
                {
                    ParticipantName = request.ParticipantName?.Trim()?? string.Empty,
                    users = user
                };
            }

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            HttpContext.Session.SetInt32("UserId", user.UsersId);
            HttpContext.Session.SetString("Role", role);

            return Ok(new { user.UsersId, user.UserName, user.Email, Role = role });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u =>
              u.Email == request.Identifier || u.UserName == request.Identifier);

            if (user == null ||
              !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized("Invalid username/email or password.");

            HttpContext.Session.SetInt32("UserId", user.UsersId);
            HttpContext.Session.SetString("Role", user.role ?? "");

            return Ok(new { user.UsersId, user.UserName, user.Email, Role = user.role });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Ok(new { message = "Logged out." });
        }
    }
}
