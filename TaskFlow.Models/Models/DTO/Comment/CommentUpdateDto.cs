using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.Models.DTO.Comment
{
    public class CommentUpdateDto
    {
        [Required, MaxLength(500)]
        public string Content { get; set; } = null!;

        public int? ProjectId { get; set; }
        public int? TaskItemId { get; set; }
    }
}
