using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Models.Models;
using TaskFlow.Models.Models.DTO.Project;
using TaskFlow.Utility.Interface;
using TaskFlow.Utility.Job;

namespace TaskFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectController(IProjectRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;

        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var projects = await _repo.GetAllAsync();

            if (projects == null || !projects.Any())
                return NotFound("No projects found.");

            // Map entity → DTO
            // Limit the fields returned in the API response
            var projectDtos = projects.Select(p => new ProjectGetDto
            {
                Id = p.Id,
                ProjectCode = p.ProjectCode,
                Name = p.Name,
                Deadline = p.Deadline,
                EstimatedTime = p.EstimatedTime,
                ProjectManagerId = p.ProjectManagerId,
                Progress = (p.Tasks != null && p.Tasks.Any())
                    ? (int)p.Tasks.Average(t => t.Progress)
                    : 0
            }).ToList();

            return Ok(projectDtos);
        }
        // GET by id:
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            var dto = new ProjectGetDto
            {
                Id = project.Id,
                ProjectCode = project.ProjectCode,
                Name = project.Name,
                Deadline = project.Deadline,
                EstimatedTime = project.EstimatedTime,
                ProjectManagerId = project.ProjectManagerId,
                Progress = (project.Tasks != null && project.Tasks.Any())
                    ? (int)project.Tasks.Average(t => t.Progress)
                    : 0
            };

            return Ok(dto);
        }
        // POST: 
        [HttpPost]
        //[Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Create([FromBody] ProjectCreateDto dto)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = new Project
            {
                ProjectCode = dto.ProjectCode,
                Name = dto.Name,
                Deadline = dto.Deadline,
                EstimatedTime = dto.EstimatedTime,
                ProjectManagerId = User.IsInRole("Administrator") ? dto.ProjectManagerId : currentUser.Id
            };

            await _repo.AddAsync(project);

            // Schedule reminder job using Hangfire
            var reminderTime = project.Deadline.AddDays(-5);
            if (reminderTime > DateTime.UtcNow && !string.IsNullOrEmpty(project.ProjectManager?.Email))
            {
                var jobId = BackgroundJob.Schedule<ProjectEndReminderJob>(
                    job => job.SendReminderAsync(project.Id),
                    reminderTime - DateTime.UtcNow
                );
                project.ReminderJobId = jobId;
                project.ReminderSent = false;
                await _repo.UpdateReminderJobIdAsync(project.Id, jobId);
            }

            var resultDto = new ProjectGetDto
            {
                Id = project.Id,
                ProjectCode = project.ProjectCode,
                Name = project.Name,
                Deadline = project.Deadline,
                EstimatedTime = project.EstimatedTime,
                ProjectManagerId = project.ProjectManagerId,
                Progress = 0
            };

            return CreatedAtAction(nameof(GetById), new { id = project.Id }, resultDto);
        }
        // PUT: 
        [HttpPut("{id}")]
        //[Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (User.IsInRole("ProjectManager") && project.ProjectManagerId != currentUser.Id)
                return Forbid();

            project.Name = dto.Name;
            project.Deadline = dto.Deadline;
            project.EstimatedTime = dto.EstimatedTime;

            await _repo.UpdateAsync(project);

            return NoContent();
        }
        // DELETE: 
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (User.IsInRole("ProjectManager") && project.ProjectManagerId != currentUser.Id)
                return Forbid();

            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
