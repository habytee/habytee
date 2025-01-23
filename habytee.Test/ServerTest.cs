using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using habytee.Server.DataAccess;
using habytee.Interconnection.Models;
using System.Linq;
using Moq;
using habytee.Client.Services;
using habytee.Client.ViewModels;
using System.Net.Http.Json;

namespace habytee.Tests.DataAccess
{
    public class WriteDbContextTests
    {
        private WriteDbContext dbContext;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<WriteDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            dbContext = new WriteDbContext(options);
        }

        [Test]
        public void CanAddUser()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };

            // Act
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // Assert
            Assert.AreEqual(1, dbContext.Users.Count());
            Assert.AreEqual("test@example.com", dbContext.Users.First().Email);
        }

        [Test]
        public void CanAddHabitToUser()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var habit = new Habit { Name = "Test Habit", UserId = user.Id };

            // Act
            dbContext.Habits.Add(habit);
            dbContext.SaveChanges();

            // Assert
            Assert.AreEqual(1, dbContext.Habits.Count());
            Assert.AreEqual("Test Habit", dbContext.Habits.First().Name);
            Assert.AreEqual(user.Id, dbContext.Habits.First().UserId);
        }

        [Test]
        public void CanAddHabitCheckedEvent()
        {
            // Arrange
            var user = new User { Email = "test@example.com" };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var habit = new Habit { Name = "Test Habit", UserId = user.Id };
            dbContext.Habits.Add(habit);
            dbContext.SaveChanges();

            var habitCheckedEvent = new HabitCheckedEvent { HabitId = habit.Id, TimeStamp = DateTime.Now };

            // Act
            dbContext.HabitCheckedEvents.Add(habitCheckedEvent);
            dbContext.SaveChanges();

            // Assert
            Assert.AreEqual(1, dbContext.HabitCheckedEvents.Count());
            Assert.AreEqual(habit.Id, dbContext.HabitCheckedEvents.First().HabitId);
        }
    }
}
