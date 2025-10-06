using Xunit;
using Moq;
using TaskFlow.Utility.Interface;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Job;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Data;
using System;
using System.Threading.Tasks;

namespace TaskFlow.Test
{
    public class ProjectEndReminderJobTest
    {
        private ApplicationDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "JobTestDb")
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        // Test to ensure SendReminderAsync sends email and marks reminder as sent
        public async Task SendReminderAsync_ShouldSendEmailAndMarkReminderSent()
        {
            var context = GetDbContext();
            var emailServiceMock = new Mock<IEmailService>();

            var project = new Project
            {
                Id = 1,
                Name = "Test Project",
                ProjectManager = new ApplicationUser { Email = "pm@test.com" },
                ProjectManagerId = "user1",
                ReminderSent = false,
                Deadline = DateTime.UtcNow.AddDays(5)
            };
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var job = new ProjectEndReminderJob(context, emailServiceMock.Object);

            await job.SendReminderAsync(1);

            emailServiceMock.Verify(x => x.SendEmailAsync(
                "pm@test.com",
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Once);

            var updatedProject = await context.Projects.FindAsync(1);
            Assert.True(updatedProject.ReminderSent);
        }
    }
}
