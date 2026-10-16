using RaceDay.DTOs1;


namespace RaceDay.DTOs1
{
    public class RegisterRequest
    {
        public string Email { get; set; } = "";
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";
        public string? ParticipantName { get; set; }
        public int? OrganiserNumber { get; set; }
    }
}