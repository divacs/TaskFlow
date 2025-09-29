using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Interface;

namespace TaskFlow.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _repo;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProjectController(IProjectRepository repo, UserManager<ApplicationUser> userManager)
        {
            _repo = repo;
            _userManager = userManager;
        }

        // GET: /Project
        public async Task<IActionResult> Index()
        {
            var projects = await _repo.GetAllAsync();

            // adding progress calculation based on tasks
            foreach (var project in projects)
            {
                if (project.Tasks != null && project.Tasks.Any())
                    project.Progress = (int)project.Tasks.Average(t => t.Progress);
                else
                    project.Progress = 0;
            }
            // Get current user for view checks
            var currentUser = await _userManager.GetUserAsync(User);
            ViewBag.CurrentUserId = currentUser.Id;

            return View(projects);
        }

        // GET: /Project/Create
        [Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Create()
        {
            if (User.IsInRole("Administrator"))
            {
                // list of Project Managers for Admin to choose from
                ViewBag.ProjectManagers = await _repo.GetAllProjectManagersAsync();
            }
            return View();
        }

        // POST: /Project/Create
        [HttpPost]
        [Authorize(Roles = "Administrator,ProjectManager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                if (User.IsInRole("Administrator"))
                    ViewBag.ProjectManagers = await _repo.GetAllProjectManagersAsync();
                return View(project);
            }

            if (User.IsInRole("Administrator"))
            {
                // Admin has to select a Project Manager
                if (string.IsNullOrEmpty(project.ProjectManagerId))
                {
                    ModelState.AddModelError("ProjectManagerId", "Please select a Project Manager.");
                    ViewBag.ProjectManagers = await _repo.GetAllProjectManagersAsync();
                    return View(project);
                }
            }
            else if (User.IsInRole("ProjectManager"))
            {
                // PM can only assign projects to themselves
                project.ProjectManagerId = currentUser.Id;
            }

            await _repo.AddAsync(project);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Project/Edit
        [Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);

            // M can only edit their own projects
            if (User.IsInRole("ProjectManager") && project.ProjectManagerId != currentUser.Id)
                return Forbid();

            if (User.IsInRole("Administrator"))
                ViewBag.ProjectManagers = await _repo.GetAllProjectManagersAsync();

            return View(project);
        }

        // POST: /Project/Edit
        [HttpPost]
        [Authorize(Roles = "Administrator,ProjectManager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Project project)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
            {
                if (User.IsInRole("Administrator"))
                    ViewBag.ProjectManagers = await _repo.GetAllProjectManagersAsync();
                return View(project);
            }

            // If it is PM, ensure they can only edit their own projects
            if (User.IsInRole("ProjectManager"))
            {
                var existing = await _repo.GetByIdAsync(project.Id);
                if (existing == null) return NotFound();

                if (existing.ProjectManagerId != currentUser.Id)
                    return Forbid();

                project.ProjectManagerId = existing.ProjectManagerId;
            }

            await _repo.UpdateAsync(project);
            return RedirectToAction(nameof(Index));
        }

        // GET: /Project/Delete
        [Authorize(Roles = "Administrator,ProjectManager")]
        public async Task<IActionResult> Delete(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);

            // PM can only delete their own projects
            if (User.IsInRole("ProjectManager") && project.ProjectManagerId != currentUser.Id)
                return Forbid();

            return View(project);
        }

        // POST: /Project/Delete
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator,ProjectManager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            // PM can only delete their own projects
            if (User.IsInRole("ProjectManager") && project.ProjectManagerId != currentUser.Id)
                return Forbid();

            await _repo.DeleteAsync(id);
            return View("DeleteConfirmed"); // succesfull deletion
        }

    }
}
