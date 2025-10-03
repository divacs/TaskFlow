using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskFlow.Models.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string ProjectCode { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public DateTime Deadline { get; set; }
        public int EstimatedTime { get; set; }

        // Project Manager (required)
        [Required]
        [ForeignKey(nameof(ProjectManager))]
        public string ProjectManagerId { get; set; }

        [ValidateNever]
        public ApplicationUser ProjectManager { get; set; }

        // Relationships
        [ValidateNever]
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        [ValidateNever]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Izračunati progress (ne čuva se u bazi)
        [NotMapped]
        public int Progress { get; set; }
        // Reminder properties for Hangfire integration
        public bool ReminderSent { get; set; } = false;   // if is reminder sent
        public string? ReminderJobId { get; set; }        // ID for Hangfire job

    }
}
