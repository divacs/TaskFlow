using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.Models.DTO.TaskItem
{
    public class TaskItemCreateDto
    {
        [Required, MaxLength(150)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime Deadline { get; set; }

        public int EstimatedTime { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.New;

        // Optional: Assigned user
        public string? AssignedUserId { get; set; }

        // Project association
        [Required]
        public int ProjectId { get; set; }
    }
}
