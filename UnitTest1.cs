using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Controllers;
using RaceDay.Data1;
using RaceDay.Models1;
using RaceDay.Service.RaceDay.Service;
using RaceDay.Tests;

namespace RaceDay.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Organiser_Can_Create_Event()
        {
            var options = new DbContextOptionsBuilder<RaceDayContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var db = new RaceDayContext(options);
            var controller = new EventsController(new EventsService(db));

            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            httpContext.Session.SetString("Role", "Organiser");

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var newEvent = new Events
            {
                EventsName = "Test Race",
                EventDate = DateTime.UtcNow,
                Description = "A test event",
                Location = "Test location",
                OrganiserId = 1
            };

            var result = await controller.Create(newEvent);

            Assert.IsType<CreatedAtActionResult>(result);
            Assert.Single(db.Events);
        }
    }
}