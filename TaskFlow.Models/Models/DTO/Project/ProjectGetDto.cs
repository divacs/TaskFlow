using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.Models.DTO.Project
{
    public class ProjectGetDto
    {
        public int Id { get; set; }
        public string ProjectCode { get; set; }
        public string Name { get; set; }
        public DateTime Deadline { get; set; }
        public int EstimatedTime { get; set; }
        public string ProjectManagerId { get; set; } 
        public int Progress { get; set; }
    }
}
