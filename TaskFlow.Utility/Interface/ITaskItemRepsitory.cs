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

        // maybe useful for updating status and progress separately
        Task UpdateStatusAsync(int id, string status);
        Task UpdateProgressAsync(int id, int progress);
    }
}
