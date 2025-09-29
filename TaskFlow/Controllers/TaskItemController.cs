using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Interface;

namespace TaskFlow.Controllers
{
    [Authorize]
    public class TaskItemController : Controller
    {
        private readonly ITaskItemRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public TaskItemController(ITaskItemRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var allTasks = await _repo.GetAllAsync();
            var userId = GetCurrentUserId();

            if (User.IsInRole("Administrator") || User.IsInRole("ProjectManager"))
                return View(allTasks);

            // Developers see only their tasks and unassigned tasks
            var filtered = allTasks.Where(t => t.AssignedUserId == null || t.AssignedUserId == userId);
            return View(filtered);
        }

        // GET: Create Task - just Admin and PM
        [Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Create()
        {
            // Developers dropdown
            var developers = await _userManager.GetUsersInRoleAsync("Developer")
                             ?? new List<ApplicationUser>();
            ViewBag.Developers = developers.Select(d => new SelectListItem
            {
                Value = d.Id,
                Text = d.FullName ?? d.UserName
            }).ToList();

            // Projects dropdown
            var projects = await _repo.GetAllProjectsAsync(); // Metoda koju treba dodati u repo
            ViewBag.Projects = projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            return View(new TaskItem());
        }



        // POST: Create Task
        [HttpPost]
        [Authorize(Roles = "Administrator,ProjectManager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem taskItem)
        {
            // Validate ProjectId exists
            var projects = await _repo.GetAllProjectsAsync();
            if (!projects.Any(p => p.Id == taskItem.ProjectId))
            {
                ModelState.AddModelError("ProjectId", "Selected project does not exist.");
            }

            // Reload dropdowns
            var developers = await _userManager.GetUsersInRoleAsync("Developer")
                             ?? new List<ApplicationUser>();
            ViewBag.Developers = developers.Select(d => new SelectListItem
            {
                Value = d.Id,
                Text = d.FullName ?? d.UserName
            }).ToList();

            ViewBag.Projects = projects.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            if (!ModelState.IsValid)
                return View(taskItem);

            // Check developer active task limit (Progress < 100)
            if (!string.IsNullOrEmpty(taskItem.AssignedUserId))
            {
                int activeTasks = (await _repo.GetAllAsync())
                    .Count(t => t.AssignedUserId == taskItem.AssignedUserId && t.Progress < 100);

                if (activeTasks >= 3)
                {
                    ModelState.AddModelError("AssignedUserId", "This developer already has 3 active tasks assigned.");
                    return View(taskItem);
                }
            }

            await _repo.AddAsync(taskItem);
            return RedirectToAction(nameof(Index));
        }


        [Authorize(Roles = "Administrator,ProjectManager,Developer")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            var currentUserId = GetCurrentUserId();

            if (User.IsInRole("Developer") && task.AssignedUserId != currentUserId)
                return Forbid();

            if (!User.IsInRole("Developer"))
            {
                var developers = await _userManager.GetUsersInRoleAsync("Developer");
                ViewBag.Developers = developers ?? new List<ApplicationUser>();
            }

            return View(task);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,ProjectManager,Developer")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(TaskItem taskItem)
        {
            if (!ModelState.IsValid) return View(taskItem);

            var existingTask = await _repo.GetByIdAsync(taskItem.Id);
            if (existingTask == null) return NotFound();

            var currentUserId = GetCurrentUserId();

            if (User.IsInRole("Developer"))
            {
                if (existingTask.AssignedUserId != currentUserId)
                    return Forbid();

                existingTask.Status = taskItem.Status;
                existingTask.Progress = taskItem.Progress;
                existingTask.Description = taskItem.Description;
            }
            else
            {
                existingTask.Title = taskItem.Title;
                existingTask.Description = taskItem.Description;
                existingTask.Deadline = taskItem.Deadline;
                existingTask.EstimatedTime = taskItem.EstimatedTime;
                existingTask.Progress = taskItem.Progress;
                existingTask.Status = taskItem.Status;

                if (taskItem.AssignedUserId != existingTask.AssignedUserId)
                {
                    if (string.IsNullOrEmpty(taskItem.AssignedUserId))
                        await _repo.UnassignTaskAsync(existingTask.Id);
                    else
                    {
                        bool assigned = await _repo.AssignTaskAsync(existingTask.Id, taskItem.AssignedUserId, "Developer");
                        if (!assigned)
                        {
                            ModelState.AddModelError("AssignedUserId", "This developer already has 3 tasks assigned.");
                            var developers = await _userManager.GetUsersInRoleAsync("Developer");
                            ViewBag.Developers = developers ?? new List<ApplicationUser>();
                            return View(taskItem);
                        }
                    }
                }
            }

            await _repo.UpdateAsync(existingTask);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,ProjectManager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            await _repo.DeleteAsync(id);

            // return to DeleteConfirmed view that shows deleted task details
            return View("DeleteConfirmed", task);
        }


        private string GetCurrentUserId()
        {
            return User.Claims.First(c => c.Type == "sub" || c.Type.Contains("nameidentifier")).Value;
        }
    }
}
