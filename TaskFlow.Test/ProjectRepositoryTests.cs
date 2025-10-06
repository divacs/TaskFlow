using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Repository;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskFlow.Test
{
    public class ProjectRepositoryTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        // Test to ensure AddAsync adds a project
        public async Task AddAsync_ShouldAddProject()
        {
            var context = GetDbContext();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            var repo = new ProjectRepository(context, userManagerMock.Object);

            var project = new Project { Id = 1, Name = "Test Project", ProjectCode = "TP001", ProjectManagerId = "user1" };
            await repo.AddAsync(project);

            var added = await context.Projects.FindAsync(1);
            Assert.NotNull(added);
            Assert.Equal("Test Project", added.Name);
        }

        [Fact]
        // Test to ensure GetAllAsync returns all projects
        public async Task UpdateReminderJobIdAsync_ShouldUpdateJobId()
        {
            var context = GetDbContext();
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            var repo = new ProjectRepository(context, userManagerMock.Object);

            var project = new Project { Id = 1, Name = "Test Project", ProjectCode = "TP001", ProjectManagerId = "user1" };
            await repo.AddAsync(project);

            await repo.UpdateReminderJobIdAsync(1, "job123");

            var updated = await context.Projects.FindAsync(1);
            Assert.Equal("job123", updated.ReminderJobId);
            Assert.False(updated.ReminderSent); // should reset ReminderSent
        }
    }
}
