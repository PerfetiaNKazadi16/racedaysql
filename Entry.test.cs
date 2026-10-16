using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Controllers;
using RaceDay.Data1;
using RaceDay.Models1;
using RaceDay.Service;
using Xunit;

namespace RaceDay.Tests
{
    public class EntryTests
    {
        [Fact]
        public async Task Participant_Can_Enrol_And_Entry_Is_Saved()

        {
            var options = new DbContextOptionsBuilder<RaceDayContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var db = new RaceDayContext(options);

            var participantUser = new Users
            {
                UserName = "runner1",
                Email = "runner1@test.com",
                role = "Participant",
                AccountType = "Standard",
                PasswordHash = "test-hash"
            };
            db.Users.Add(participantUser);
            await db.SaveChangesAsync();

            var participant = new Participant
            {
                ParticipantName = "Test Runner",
                UserId = participantUser.UsersId,
                users = participantUser
            };
            db.Participants.Add(participant);

            var organiserUser = new Users
            {
                UserName = "organiser1",
                Email = "organiser1@test.com",
                role = "Organiser",
                AccountType = "Organiser",
                PasswordHash = "test-hash"
            };
            db.Users.Add(organiserUser);
            await db.SaveChangesAsync();

            var organiser = new Organiser
            {
                OrganiserNumber = 1001,
                UserId = organiserUser.UsersId,
                Users = organiserUser
            };
            db.Organisers.Add(organiser);
            await db.SaveChangesAsync();

            var raceEvent = new Events
            {
                EventsName = "Test Race",
                EventDate = DateTime.UtcNow,
                Description = "Test event",
                Location = "Test location",
                OrganiserId = organiser.OrganiserId,
                Organiser = organiser
            };
            db.Events.Add(raceEvent);
            await db.SaveChangesAsync();

            var category = new Category
            {
                CategoryName = "10km",
                EventId = raceEvent.EventsId
            };
            db.Categories.Add(category);
            await db.SaveChangesAsync();

            var controller = new EntryController(new EntryService(db));
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();
            httpContext.Session.SetString("Role", "Participant");
            httpContext.Session.SetInt32("UserId", participantUser.UsersId);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var entry = new Entry
            {
                EventId = raceEvent.EventsId,
                CategoryId = category.CategoryId,
                Price = 0
            };

            var result = await controller.Enrol(entry);

            Assert.IsType<OkObjectResult>(result);

            var savedEntry = await db.Entries.SingleAsync();
            Assert.Equal(participant.ParticipantId, savedEntry.ParticipantId);
            Assert.Equal(raceEvent.EventsId, savedEntry.EventId);
            Assert.Equal(category.CategoryId, savedEntry.CategoryId);

            var duplicateResult = await controller.Enrol(new Entry
            {
                EventId = raceEvent.EventsId,
                CategoryId = category.CategoryId,
                Price = 0
            });

            Assert.IsType<BadRequestObjectResult>(duplicateResult);
            Assert.Single(db.Entries);
        }
    }
}