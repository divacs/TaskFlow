using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Models.Models;
using TaskFlow.Models.Models.DTO.Comment;
using TaskFlow.Utility.Interface;

namespace TaskFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _repo;

        public CommentController(ICommentRepository repo)
        {
            _repo = repo;
        }

        // GET: 
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _repo.GetAllAsync();

            var result = comments.Select(c => new CommentGetDto
            {
                Id = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                ProjectId = c.ProjectId,
                TaskItemId = c.TaskItemId
            });

            return Ok(result);
        }

        // GET by id: 
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _repo.GetByIdAsync(id);
            if (comment == null)
                return NotFound(new { message = $"Comment with ID {id} not found." });

            var dto = new CommentGetDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                ProjectId = comment.ProjectId,
                TaskItemId = comment.TaskItemId
            };

            return Ok(dto);
        }

        // POST: 
        [HttpPost]
        //[Authorize(Roles = "Administrator,ProjectManager,Developer")]
        public async Task<IActionResult> Create([FromBody] CommentGetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var newComment = new Comment
            {
                Content = dto.Content,
                ProjectId = dto.ProjectId,
                TaskItemId = dto.TaskItemId,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(newComment);

            var result = new CommentGetDto
            {
                Id = newComment.Id,
                Content = newComment.Content,
                CreatedAt = newComment.CreatedAt,
                ProjectId = newComment.ProjectId,
                TaskItemId = newComment.TaskItemId
            };

            return CreatedAtAction(nameof(GetById), new { id = newComment.Id }, result);
        }

        // PUT: 
        [HttpPut("{id}")]
        //[Authorize(Roles = "Administrator,ProjectManager,Developer")]
        public async Task<IActionResult> Update(int id, [FromBody] CommentGetDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Comment with ID {id} not found." });

            existing.Content = dto.Content;
            existing.ProjectId = dto.ProjectId;
            existing.TaskItemId = dto.TaskItemId;

            await _repo.UpdateAsync(existing);

            var updated = new CommentGetDto
            {
                Id = existing.Id,
                Content = existing.Content,
                CreatedAt = existing.CreatedAt,
                ProjectId = existing.ProjectId,
                TaskItemId = existing.TaskItemId
            };

            return Ok(updated);
        }

        // DELETE: 
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Comment with ID {id} not found." });

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
