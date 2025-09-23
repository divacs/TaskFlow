using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TaskFlow.Models.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string ProjectCode { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Name { get; set; } = null!;

        public DateTime Deadline { get; set; }
        public int EstimatedTime { get; set; }

        // relationships
        [ValidateNever] // to avoid circular reference during model validation
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

        [ValidateNever]
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
