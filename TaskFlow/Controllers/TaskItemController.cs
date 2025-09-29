using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            if (User.IsInRole("Administrator") || User.IsInRole("ProjectManager"))
                return View(allTasks);

            var userId = GetCurrentUserId();
            var filtered = allTasks.Where(t => t.AssignedUserId == null || t.AssignedUserId == userId);
            return View(filtered);
        }

        [Authorize(Roles = "Administrator,ProjectManager")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Administrator,ProjectManager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskItem taskItem)
        {
            if (!ModelState.IsValid) return View(taskItem);

            await _repo.AddAsync(taskItem);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator,ProjectManager,Developer")]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            if (User.IsInRole("Developer") && task.AssignedUserId != GetCurrentUserId())
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

            if (User.IsInRole("Developer"))
            {
                if (existingTask.AssignedUserId != GetCurrentUserId())
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

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator,ProjectManager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private string GetCurrentUserId()
        {
            return User.Claims.First(c => c.Type == "sub" || c.Type.Contains("nameidentifier")).Value;
        }
    }
}
