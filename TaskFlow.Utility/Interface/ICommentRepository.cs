using TaskFlow.Models.Models;

namespace TaskFlow.Utility.Interface
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<IEnumerable<Comment>> GetByProjectIdAsync(int projectId);
        Task<IEnumerable<Comment>> GetByTaskItemIdAsync(int taskItemId);
        Task AddAsync(Comment comment);
        Task DeleteAsync(int id);
    }
}
