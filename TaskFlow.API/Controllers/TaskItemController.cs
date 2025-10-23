using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Models.Models;
using TaskFlow.Models.Models.DTO.TaskItem;
using TaskFlow.Utility.Interface;

namespace TaskFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskItemController(ITaskItemRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }
        // GET:
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allTasks = await _repo.GetAllAsync();
            var userId = GetCurrentUserId();

            IEnumerable<TaskItem> filteredTasks;

            if (User.IsInRole("Administrator") || User.IsInRole("ProjectManager"))
                filteredTasks = allTasks;
            else
                filteredTasks = allTasks.Where(t => t.AssignedUserId == null || t.AssignedUserId == userId);

            var dtoList = filteredTasks.Select(t => new TaskItemGetDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Deadline = t.Deadline,
                EstimatedTime = t.EstimatedTime,
                Status = t.Status,
                Progress = t.Progress,
                ProjectId = t.ProjectId,
                AssignedUserId = t.AssignedUserId
            }).ToList();

            return Ok(dtoList);
        }
        // GET by id: 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            var dto = new TaskItemGetDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                EstimatedTime = task.EstimatedTime,
                Status = task.Status,
                Progress = task.Progress,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedUserId
            };

            return Ok(dto);
        }
        // POST: 
        [HttpPost]
        [Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Create([FromBody] TaskItemCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var task = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Deadline = dto.Deadline,
                EstimatedTime = dto.EstimatedTime,
                Status = dto.Status,
                Progress = 0,
                ProjectId = dto.ProjectId,
                AssignedUserId = dto.AssignedUserId
            };

            // Check assigned developer limit
            if (!string.IsNullOrEmpty(task.AssignedUserId))
            {
                var activeTasks = (await _repo.GetAllAsync())
                    .Count(t => t.AssignedUserId == task.AssignedUserId && t.Progress < 100);

                if (activeTasks >= 3)
                    return BadRequest("This developer already has 3 active tasks assigned.");
            }

            await _repo.AddAsync(task);

            var resultDto = new TaskItemGetDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                EstimatedTime = task.EstimatedTime,
                Status = task.Status,
                Progress = task.Progress,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedUserId
            };

            return CreatedAtAction(nameof(GetById), new { id = task.Id }, resultDto);
        }
        // PUT: api/TaskItemApi/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator,ProjectManager,Developer")]
        public async Task<IActionResult> Update(int id, [FromBody] TaskItemUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            var currentUserId = GetCurrentUserId();

            if (User.IsInRole("Developer") && task.AssignedUserId != currentUserId)
                return Forbid();

            if (User.IsInRole("Developer"))
            {
                task.Status = dto.Status;
                task.Progress = dto.Progress;
                task.Description = dto.Description;
            }
            else
            {
                task.Title = dto.Title;
                task.Description = dto.Description;
                task.Deadline = dto.Deadline;
                task.EstimatedTime = dto.EstimatedTime;
                task.Progress = dto.Progress;
                task.Status = dto.Status;

                // Assign developer if needed
                if (dto.AssignedUserId != task.AssignedUserId)
                {
                    if (string.IsNullOrEmpty(dto.AssignedUserId))
                        await _repo.UnassignTaskAsync(task.Id);
                    else
                    {
                        bool assigned = await _repo.AssignTaskAsync(task.Id, dto.AssignedUserId, "Developer");
                        if (!assigned)
                            return BadRequest("This developer already has 3 tasks assigned.");
                        task.AssignedUserId = dto.AssignedUserId;
                    }
                }
            }

            await _repo.UpdateAsync(task);
            return NoContent();
        }

        // DELETE: api/TaskItemApi/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            await _repo.DeleteAsync(id);
            return NoContent();
        }

        private string GetCurrentUserId()
        {
            // Za test: vraća dummy ID
            //return "test-user-id";
            return User.Claims.First(c => c.Type == "sub" || c.Type == ClaimTypes.NameIdentifier).Value;
        }

    }
}
