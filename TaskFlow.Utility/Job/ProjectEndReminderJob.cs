using TaskFlow.Models.Models;
using TaskFlow.Utility.Interface;
using TaskFlow.Data;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Utility.Job
{
    public class ProjectEndReminderJob : IProjectEndReminderJob
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public ProjectEndReminderJob(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task SendRemindersAsync()
        {
            var now = DateTime.UtcNow;
            var reminderDate = now.AddDays(5); // 5 days before deadline

            // take projects that have deadline in 5 days
            var projects = await _context.Projects
                .Include(p => p.ProjectManager)
                .Where(p =>
                    p.Deadline.Date == reminderDate.Date)
                .ToListAsync();

            foreach (var project in projects)
            {
                if (project.ProjectManager?.Email == null)
                    continue;

                await _emailService.SendEmailAsync(
                    project.ProjectManager.Email,
                    "Project Deadline Approaching",
                    $"Reminder: The project '{project.Name}' (Code: {project.ProjectCode}) is due on {project.Deadline:dd.MM.yyyy}."
                );
            }
        }

        public async Task SendReminderAsync(int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectManager)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null || project.ProjectManager?.Email == null)
                return;

            await _emailService.SendEmailAsync(
                project.ProjectManager.Email,
                "Project Deadline Approaching",
                $"Reminder: The project '{project.Name}' (Code: {project.ProjectCode}) is due on {project.Deadline:dd.MM.yyyy}."
            );
        }
    }
}
