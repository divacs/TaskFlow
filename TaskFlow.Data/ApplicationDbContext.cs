using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Models.Models;
using TaskStatus = TaskFlow.Models.Models.TaskStatus;

namespace TaskFlow.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Users (Project Managers)
            modelBuilder.Entity<ApplicationUser>().HasData(
                new ApplicationUser
                {
                    Id = "PM001",
                    UserName = "pm1",
                    NormalizedUserName = "PM1",
                    Email = "pm1@taskflow.com",
                    NormalizedEmail = "PM1@TASKFLOW.COM",
                    EmailConfirmed = true,
                    FullName = "Alice ProjectManager",
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Pm123!")
                },
                new ApplicationUser
                {
                    Id = "PM002",
                    UserName = "pm2",
                    NormalizedUserName = "PM2",
                    Email = "pm2@taskflow.com",
                    NormalizedEmail = "PM2@TASKFLOW.COM",
                    EmailConfirmed = true,
                    FullName = "Bob ProjectManager",
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Pm123!")
                },
                new ApplicationUser
                {
                    Id = "PM003",
                    UserName = "pm3",
                    NormalizedUserName = "PM3",
                    Email = "pm3@taskflow.com",
                    NormalizedEmail = "PM3@TASKFLOW.COM",
                    EmailConfirmed = true,
                    FullName = "Charlie ProjectManager",
                    PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null, "Pm123!")
                }
            );

            // Seed Projects
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    ProjectCode = "PRJ001",
                    Name = "Website Redesign",
                    Deadline = new DateTime(2025, 12, 31),
                    EstimatedTime = 200,
                    ProjectManagerId = "PM001"
                },
                new Project
                {
                    Id = 2,
                    ProjectCode = "PRJ002",
                    Name = "Mobile App Development",
                    Deadline = new DateTime(2025, 11, 15),
                    EstimatedTime = 350,
                    ProjectManagerId = "PM002"
                },
                new Project
                {
                    Id = 3,
                    ProjectCode = "PRJ003",
                    Name = "Internal CRM System",
                    Deadline = new DateTime(2026, 01, 30),
                    EstimatedTime = 500,
                    ProjectManagerId = "PM003"
                }
            );

            // Seed TaskItems
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem
                {
                    Id = 1,
                    Title = "Design new homepage",
                    Description = "Create wireframes and mockups",
                    Deadline = new DateTime(2025, 06, 30),
                    EstimatedTime = 40,
                    Status = TaskStatus.InProgress,
                    Progress = 50,
                    ProjectId = 1,
                    AssignedUserId = null
                },
                new TaskItem
                {
                    Id = 2,
                    Title = "Implement login feature",
                    Description = "Add JWT authentication in backend",
                    Deadline = new DateTime(2025, 07, 15),
                    EstimatedTime = 60,
                    Status = TaskStatus.New,
                    Progress = 0,
                    ProjectId = 2,
                    AssignedUserId = null
                },
                new TaskItem
                {
                    Id = 3,
                    Title = "Database schema design",
                    Description = "Plan and implement core tables",
                    Deadline = new DateTime(2025, 05, 20),
                    EstimatedTime = 30,
                    Status = TaskStatus.Finished,
                    Progress = 100,
                    ProjectId = 3,
                    AssignedUserId = null
                }
            );

            // Seed Comments
            modelBuilder.Entity<Comment>().HasData(
                new Comment
                {
                    Id = 1,
                    Content = "Looks good so far!",
                    CreatedAt = DateTime.UtcNow,
                    ProjectId = 1
                },
                new Comment
                {
                    Id = 2,
                    Content = "We should adjust the deadline",
                    CreatedAt = DateTime.UtcNow,
                    TaskItemId = 1
                },
                new Comment
                {
                    Id = 3,
                    Content = "Backend authentication pending",
                    CreatedAt = DateTime.UtcNow,
                    ProjectId = 2
                }
            );
        }
    }
}
