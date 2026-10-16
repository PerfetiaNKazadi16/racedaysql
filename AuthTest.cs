using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.Controllers;
using RaceDay.Data1;
using RaceDay.Models1;
using RaceDay.Tests;
using RaceDay.DTOs1;

namespace RaceDay.Test
{
    public class AuthTest
    {
        [Fact]
        public async Task Login_With_Correct_Password_Sets_User_Session()
        {
            var options = new DbContextOptionsBuilder<RaceDayContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var db = new RaceDayContext(options);

            var user = new Users
            {
                UserName = "runner",
                Email = "runner@test.com",
                role = "Participant",
                AccountType = "Standard",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("RaceDay123!")
            };

            db.Users.Add(user);
            await db.SaveChangesAsync();

            var controller = new AuthController(db);
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var request = new LoginRequest
            {
                Identifier = "runner@test.com",
                Password = "RaceDay123!"
            };

            var result = await controller.Login(request);

            Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user.UsersId, httpContext.Session.GetInt32("UserId"));
            Assert.Equal("Participant", httpContext.Session.GetString("Role"));
        }


        [Fact]
        public async Task Login_With_Wrong_Password_Is_Rejected()
        {
            var options = new DbContextOptionsBuilder<RaceDayContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var db = new RaceDayContext(options);

            db.Users.Add(new Users
            {
                UserName = "runner",
                Email = "runner@test.com",
                role = "Participant",
                AccountType = "Standard",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("RaceDay123!")
            });

            await db.SaveChangesAsync();

            var controller = new AuthController(db);
            var httpContext = new DefaultHttpContext();
            httpContext.Session = new TestSession();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.Login(new LoginRequest
            {
                Identifier = "runner@test.com",
                Password = "WrongPassword!"
            });

            var statusResult =
                Assert.IsAssignableFrom<Microsoft.AspNetCore.Mvc.Infrastructure.IStatusCodeActionResult>(result);

            Assert.Equal(401, statusResult.StatusCode);
        }
    }
}