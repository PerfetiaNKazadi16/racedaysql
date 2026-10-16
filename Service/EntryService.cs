using Microsoft.EntityFrameworkCore;
using RaceDay.Data1;
using RaceDay.Models1;

namespace RaceDay.Service
{
    public class EntryService
    {
        private readonly RaceDayContext _db;

        public EntryService(RaceDayContext db)
        {
            _db = db;
        }

        public async Task<Entry?> EnrolAsync(int userId, Entry entry)
        {
            var participant = await _db.Participants
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (participant == null)
                return null;

            var categoryBelongsToEvent = await _db.Categories.AnyAsync(c =>
                c.CategoryId == entry.CategoryId &&
                c.EventId == entry.EventId);

            if (!categoryBelongsToEvent)
                return null;

            var alreadyEntered = await _db.Entries.AnyAsync(e =>
                e.EventId == entry.EventId &&
                e.ParticipantId == participant.ParticipantId);

            if (alreadyEntered)
                return null;

            entry.ParticipantId = participant.ParticipantId;

            _db.Entries.Add(entry);
            await _db.SaveChangesAsync();

            return entry;
        }

        public async Task<List<object>> GetForOrganiserAsync(int userId)
        {
            return await _db.Entries
                .AsNoTracking()
                .Where(e => e.Event.Organiser.UserId == userId)
                .Select(e => new
                {
                    e.EntryId,
                    e.EventId,
                    EventName = e.Event.EventsName,
                    e.ParticipantId,
                    ParticipantName = e.Participant.ParticipantName,
                    e.CategoryId,
                    CategoryName = e.Category.CategoryName,
                    e.Price
                })
                .ToListAsync<object>();
        }
    }
}

