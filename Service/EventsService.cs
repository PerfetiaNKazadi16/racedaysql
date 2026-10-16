namespace RaceDay.Service
{
    using global::RaceDay.Data1;
    using global::RaceDay.Models1;
    using Microsoft.EntityFrameworkCore;
 
    namespace RaceDay.Service
    {
        public class EventsService
        {
            private readonly RaceDayContext _db;

            public EventsService(RaceDayContext db)
            {
                _db = db;
            }

            public async Task<List<Events>> GetAllAsync()
            {
                return await _db.Events
                    .AsNoTracking()
                    .ToListAsync();
            }

            public async Task<Events?> GetByIdAsync(int id)
            {
                return await _db.Events
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.EventsId == id);
            }

            public async Task<Events> CreateAsync(Events newEvent)
            {
                _db.Events.Add(newEvent);
                await _db.SaveChangesAsync();
                return newEvent;
            }

            public async Task<bool> UpdateAsync(int id, Events updatedEvent)
            {
                var existingEvent = await _db.Events.FindAsync(id);

                if (existingEvent == null)
                    return false;

                existingEvent.EventsName = updatedEvent.EventsName;
                existingEvent.EventDate = updatedEvent.EventDate;
                existingEvent.Description = updatedEvent.Description;
                existingEvent.Location = updatedEvent.Location;
                existingEvent.EventType = updatedEvent.EventType;
                existingEvent.OrganiserId = updatedEvent.OrganiserId;

                await _db.SaveChangesAsync();
                return true;
            }

            public async Task<bool> DeleteAsync(int id)
            {
                var existingEvent = await _db.Events.FindAsync(id);

                if (existingEvent == null)
                    return false;

                _db.Events.Remove(existingEvent);
                await _db.SaveChangesAsync();
                return true;
            }
        }
    }
}
