using Microsoft.AspNetCore.Mvc;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Interface;

namespace TaskFlow.Controllers
{
    public class TaskItemController : Controller
    {
        private readonly ITaskItemRepository _repo;

        public TaskItemController(ITaskItemRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var tasks = await _repo.GetAllAsync();
            return View(tasks);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskItem taskItem)
        {
            if (!ModelState.IsValid) return View(taskItem);

            await _repo.AddAsync(taskItem);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TaskItem taskItem)
        {
            if (!ModelState.IsValid) return View(taskItem);

            await _repo.UpdateAsync(taskItem);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var task = await _repo.GetByIdAsync(id);
            if (task == null) return NotFound();

            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
