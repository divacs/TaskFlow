using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } 

        // Navigation properties
        public ICollection<Project>? ManagedProjects { get; set; } = new List<Project>();
        public ICollection<TaskItem>? AssignedTasks { get; set; } = new List<TaskItem>();
    }
}
