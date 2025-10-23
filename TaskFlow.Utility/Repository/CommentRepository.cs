using Microsoft.EntityFrameworkCore;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Interface;
using TaskFlow.Data;

namespace TaskFlow.Utility.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments
                .Include(c => c.Project)
                .Include(c => c.TaskItem)
                .ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _context.Comments
                .Include(c => c.Project)
                .Include(c => c.TaskItem)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetByProjectIdAsync(int projectId)
        {
            return await _context.Comments
                .Where(c => c.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetByTaskItemIdAsync(int taskItemId)
        {
            return await _context.Comments
                .Where(c => c.TaskItemId == taskItemId)
                .ToListAsync();
        }

        public async Task AddAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var comment = await GetByIdAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
        }
    }
}
