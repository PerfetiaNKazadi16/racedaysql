using Microsoft.EntityFrameworkCore;
using RaceDay.Data1;
using RaceDay.Models1;

namespace RaceDay.Service
{
    public class ResultService
    {
        private readonly RaceDayContext _db;

        public ResultService(RaceDayContext db)
        {
            _db = db;
        }

        public async Task<Result?> AddAsync(int userId, Result result)
        {
            var eventIsOwnedByOrganiser = await _db.Events.AnyAsync(e =>
                e.EventsId == result.EventId &&
                e.Organiser.UserId == userId);

            if (!eventIsOwnedByOrganiser)
                return null;

            var participantIsEntered = await _db.Entries.AnyAsync(e =>
                e.EventId == result.EventId &&
                e.ParticipantId == result.ParticipantId);

            if (!participantIsEntered)
                return null;

            var alreadyHasResult = await _db.Results.AnyAsync(r =>
                r.EventId == result.EventId &&
                r.ParticipantId == result.ParticipantId);

            if (alreadyHasResult)
                return null;

            _db.Results.Add(result);
            await _db.SaveChangesAsync();

            return result;
        }

        public async Task<List<object>> GetForParticipantAsync(int userId)
        {
            return await _db.Results
                .AsNoTracking()
                .Where(r => r.Participant.UserId == userId)
                .Select(r => new
                {
                    r.ResultId,
                    r.EventId,
                    EventName = r.Event.EventsName,
                    r.FinishingTime,
                    r.Position
                })
                .ToListAsync<object>();
        }
    }
}
