using TaskFlow.Models.Models;

namespace TaskFlow.Utility.Interface
{
    public interface ITaskItemRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId);
        Task AddAsync(TaskItem taskItem);
        Task UpdateAsync(TaskItem taskItem);
        Task DeleteAsync(int id);

        Task UpdateStatusAsync(int id, string status);
        Task UpdateProgressAsync(int id, int progress);

        // Assign/Unassign logic
        Task<bool> AssignTaskAsync(int taskId, string userId, string role);
        Task<bool> UnassignTaskAsync(int taskId);
    }
}
