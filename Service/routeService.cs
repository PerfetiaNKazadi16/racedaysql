using Microsoft.EntityFrameworkCore;
using RaceDay.Data1;
using RaceDay.Models1;

namespace RaceDay.Service
{
    public class RouteService
    {
        private readonly RaceDayContext _db;

        public RouteService(RaceDayContext db)
        {
            _db = db;
        }

        public async Task<List<route>> GetForEventAsync(int eventId)
        {
            return await _db.Routes
                .AsNoTracking()
                .Where(r => r.EventId == eventId)
                .Select(r => new route
                {
                    RouteId = r.RouteId,
                    EventId = r.EventId,
                    RouteName = r.RouteName,
                    Distance = r.Distance
                })
                .ToListAsync();
        }

        public async Task<route?> CreateAsync(int userId, route newRoute)
        {
            var eventIsOwnedByOrganiser = await _db.Events.AnyAsync(e =>
                e.EventsId == newRoute.EventId &&
                e.Organiser.UserId == userId);

            if (!eventIsOwnedByOrganiser)
                return null;

            var route = new route
            {
                EventId = newRoute.EventId,
                RouteName = newRoute.RouteName,
                Distance = newRoute.Distance
            };

            _db.Routes.Add(route);
            await _db.SaveChangesAsync();

            return route;
        }

        public async Task<bool> UpdateAsync(int userId, int routeId, route updated)
        {
            var existing = await _db.Routes
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.RouteId == routeId);

            if (existing == null || existing.Event.Organiser.UserId != userId)
                return false;

            existing.RouteName = updated.RouteName;
            existing.Distance = updated.Distance;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int userId, int routeId)
        {
            var existing = await _db.Routes
                .Include(r => r.Event)
                .FirstOrDefaultAsync(r => r.RouteId == routeId);

            if (existing == null || existing.Event.Organiser.UserId != userId)
                return false;

            _db.Routes.Remove(existing);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}