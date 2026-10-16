using Microsoft.EntityFrameworkCore;
using RaceDay.Data1;
using RaceDay.Models1;

namespace RaceDay.Service
{
    public class UserService
    {
        private readonly RaceDayContext _db;

        public UserService(RaceDayContext db)
        {
            _db = db;
        }

        public async Task<object?> GetProfileAsync(int userId)
        {
            return await _db.Users
                .AsNoTracking()
                .Where(u => u.UsersId == userId)
                .Select(u => new
                {
                    u.UsersId,
                    u.UserName,
                    u.Email,
                    Role = u.role,
                    ParticipantName = u.Participant == null
                        ? null
                        : u.Participant.ParticipantName,
                    OrganiserNumber = u.Organiser == null
                        ?(int?) null
                        : u.Organiser.OrganiserNumber
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateProfileAsync(int userId, Users updated)
        {
            var user = await _db.Users
                .Include(u => u.Participant)
                .Include(u => u.Organiser)
                .FirstOrDefaultAsync(u => u.UsersId == userId);

            if (user == null)
                return false;

            user.UserName = updated.UserName;
            user.Email = updated.Email;

            if (user.Participant != null && updated.Participant != null)
                user.Participant.ParticipantName =
                    updated.Participant.ParticipantName;

            if (user.Organiser != null && updated.Organiser != null)
                user.Organiser.OrganiserNumber =
                    updated.Organiser.OrganiserNumber;

            await _db.SaveChangesAsync();
            return true;
        }
    }
}