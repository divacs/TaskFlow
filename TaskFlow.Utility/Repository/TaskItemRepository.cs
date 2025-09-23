using Microsoft.EntityFrameworkCore;
using TaskFlow.Models.Models;
using TaskFlow.Utility.Interface;
using TaskFlow.Data;
using TaskStatus = TaskFlow.Models.Models.TaskStatus;

namespace TaskFlow.Utility.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.Comments)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            return await _context.TaskItems
                .Include(t => t.Project)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId)
        {
            return await _context.TaskItems
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.Comments)
                .ToListAsync();
        }

        public async Task AddAsync(TaskItem taskItem)
        {
            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            _context.TaskItems.Update(taskItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var taskItem = await GetByIdAsync(id);
            if (taskItem != null)
            {
                _context.TaskItems.Remove(taskItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            if (!Enum.TryParse<TaskStatus>(status, out var parsedStatus))
            {
                throw new ArgumentException($"Invalid status value: {status}");
            }

            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                taskItem.Status = parsedStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateProgressAsync(int id, int progress)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem != null)
            {
                taskItem.Progress = progress;
                await _context.SaveChangesAsync();
            }
        }
    }
}
