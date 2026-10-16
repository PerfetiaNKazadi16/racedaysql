using Microsoft.AspNetCore.Mvc;

namespace RaceDay.Controllers
{
    public abstract class SessionApiController : ControllerBase
    {
        protected bool TryGetUser(out int userId, out string role)
        {
            userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            role = HttpContext.Session.GetString("Role") ?? "";
            return userId != 0 && role != "";
        }

        protected IActionResult? CheckRole(string requiredRole)
        {
            if (!TryGetUser(out _, out string role))
                return Unauthorized("Please log in first.");

            if (!string.Equals(role, requiredRole, StringComparison.OrdinalIgnoreCase))
                return Forbid();

            return null;
        }
    }
}