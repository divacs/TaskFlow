using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Models.Models.DTO.Comment
{
    public class CommentGetDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int? ProjectId { get; set; }
        public int? TaskItemId { get; set; }
    }
}
