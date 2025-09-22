using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.Models
{
    internal class Comment
    {
        public int Id { get; set; }

        [Required, MaxLength(500)]
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // foreign keys and navigation properties
        [ForeignKey(nameof(Project))]
        public int? ProjectId { get; set; }

        [ValidateNever]
        public Project? Project { get; set; }

        // foren key to TaskItem
        [ForeignKey(nameof(TaskItem))]
        public int? TaskItemId { get; set; }

        [ValidateNever]
        public TaskItem? TaskItem { get; set; }
    }
}
