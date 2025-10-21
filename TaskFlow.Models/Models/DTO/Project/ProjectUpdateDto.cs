using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.Models.DTO.Project
{
    public class ProjectUpdateDto
    {

        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public DateTime Deadline { get; set; }
        public int EstimatedTime { get; set; }
    }
}
