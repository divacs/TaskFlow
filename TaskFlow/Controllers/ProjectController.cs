using Microsoft.AspNetCore.Mvc;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Interface;

namespace TaskFlow.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _repo;

        public ProjectController(IProjectRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _repo.GetAllAsync();
            return View(projects);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Project project)
        {
            if (!ModelState.IsValid) return View(project);

            await _repo.AddAsync(project);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Project project)
        {
            if (!ModelState.IsValid) return View(project);

            await _repo.UpdateAsync(project);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var project = await _repo.GetByIdAsync(id);
            if (project == null) return NotFound();

            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
