using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.Models.DTO.TaskItem
{
    public class TaskItemGetDto
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public int EstimatedTime { get; set; }
        public TaskStatus Status { get; set; }
        public int Progress { get; set; }

        public int ProjectId { get; set; }
        public string? AssignedUserId { get; set; }
    }
}
