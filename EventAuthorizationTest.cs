using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Controllers;
using RaceDay.Data1;
using RaceDay.Models1;
using RaceDay.Service;
using RaceDay.Service.RaceDay.Service;
using Xunit;

namespace RaceDay.Tests
{
    public class EventAuthorizationTests
    {
        [Fact]
        public async Task Participant_Cannot_Create_Event()
        {
            var options = new DbContextOptionsBuilder<RaceDayContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var db = new RaceDayContext(options);
            var controller = new EventsController(new EventsService(db));

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            httpContext.Session.SetString("Role", "Participant");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.Create(new Events());

            Assert.IsType<UnauthorizedResult>(result);
        }
    }

    public class TestSession : ISession
    {
        private readonly Dictionary<string, byte[]> _data = new();

        public bool IsAvailable => true;
        public string Id => "test-session";
        public IEnumerable<string> Keys => _data.Keys;

        public void Clear() => _data.Clear();
        public void Remove(string key) => _data.Remove(key);
        public void Set(string key, byte[] value) => _data[key] = value;

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_data.TryGetValue(key, out var storedValue))
            {
                value = storedValue;
                return true;
            }

            value = Array.Empty<byte>();
            return false;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;

        public Task CommitAsync(CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }
}