using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TaskFlow.Models.Models
{
    public enum TaskStatus { New, Pending, InProgress, Finished }

    public class TaskItem
    {
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime Deadline { get; set; }
        public int EstimatedTime { get; set; }

        public TaskStatus Status { get; set; } = TaskStatus.New;
        public int Progress { get; set; } = 0;

        // foreign key and navigation property
        [ForeignKey(nameof(Project))]
        public int ProjectId { get; set; }

        [ValidateNever]
        public Project Project { get; set; } = null!;

        [ValidateNever]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
