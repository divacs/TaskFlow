using Microsoft.AspNetCore.Mvc;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Interface;

namespace TaskFlow.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _repo;

        public CommentController(ICommentRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var comments = await _repo.GetAllAsync();
            return View(comments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (!ModelState.IsValid) return View(comment);

            await _repo.AddAsync(comment);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _repo.GetByIdAsync(id);
            if (comment == null) return NotFound();

            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
