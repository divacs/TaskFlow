using Xunit;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Repository;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace TaskFlow.Test
{
    public class ProjectRepositoryCrudTests
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString()) // jedinstvena baza po testu
                .Options;

            return new ApplicationDbContext(options);
        }

        private ProjectRepository GetRepository(ApplicationDbContext context)
        {
            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

            return new ProjectRepository(context, userManagerMock.Object);
        }

        [Fact]
        // Test to ensure AddAsync adds a project
        public async Task AddAsync_ShouldAddProject()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);

            var project = new Project { Id = 1, Name = "Test Project", ProjectCode = "TP001", ProjectManagerId = "user1" };
            await repo.AddAsync(project);

            var added = await context.Projects.FindAsync(1);
            Assert.NotNull(added);
            Assert.Equal("Test Project", added.Name);
        }

        [Fact]
        // Test to ensure GetAllAsync returns all projects
        public async Task GetAllAsync_ShouldReturnAllProjects()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);

            await repo.AddAsync(new Project { Id = 1, Name = "Project 1", ProjectCode = "P1", ProjectManagerId = "user1" });
            await repo.AddAsync(new Project { Id = 2, Name = "Project 2", ProjectCode = "P2", ProjectManagerId = "user2" });

            var allProjects = await repo.GetAllAsync();
            Assert.Equal(2, allProjects.Count());
        }

        [Fact]
        // Test to ensure GetByIdAsync returns the correct project
        public async Task GetByIdAsync_ShouldReturnCorrectProject()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);

            var project = new Project { Id = 1, Name = "Project 1", ProjectCode = "P1", ProjectManagerId = "user1" };
            await repo.AddAsync(project);

            var result = await repo.GetByIdAsync(1);
            Assert.NotNull(result);
            Assert.Equal("Project 1", result.Name);
        }

        [Fact]
        // Test to ensure UpdateAsync modifies the project
        public async Task UpdateAsync_ShouldModifyProject()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);

            var project = new Project { Id = 1, Name = "Old Name", ProjectCode = "P1", ProjectManagerId = "user1" };
            await repo.AddAsync(project);

            project.Name = "New Name";
            await repo.UpdateAsync(project);

            var updated = await context.Projects.FindAsync(1);
            Assert.Equal("New Name", updated.Name);
        }

        [Fact]
        // Test to ensure DeleteAsync removes the project
        public async Task DeleteAsync_ShouldRemoveProject()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);

            var project = new Project { Id = 1, Name = "Project 1", ProjectCode = "P1", ProjectManagerId = "user1" };
            await repo.AddAsync(project);

            await repo.DeleteAsync(1);

            var deleted = await context.Projects.FindAsync(1);
            Assert.Null(deleted);
        }

        [Fact]
        // Test to ensure UpdateReminderJobIdAsync updates the job ID and resets ReminderSent
        public async Task UpdateReminderJobIdAsync_ShouldUpdateJobIdAndResetReminderSent()
        {
            var context = GetDbContext();
            var repo = GetRepository(context);

            var project = new Project { Id = 1, Name = "Project 1", ProjectCode = "P1", ProjectManagerId = "user1", ReminderSent = true };
            await repo.AddAsync(project);

            await repo.UpdateReminderJobIdAsync(1, "job123");

            var updated = await context.Projects.FindAsync(1);
            Assert.Equal("job123", updated.ReminderJobId);
            Assert.False(updated.ReminderSent);
        }
    }
}
