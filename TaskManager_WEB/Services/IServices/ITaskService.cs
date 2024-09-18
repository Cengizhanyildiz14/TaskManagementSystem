using TaskManager_WEB.Models;

namespace TaskManager_WEB.Services.IServices
{
    public interface ITaskService
    {
        Task<T> GetTaskById<T>(Guid id);
        Task<T> DeleteTask<T>(Guid id);
        Task<T> CreateTask<T>(TaskCreateDto dto);
        Task<T> UpdateTask<T>(Guid id, TaskUpdateDto taskUpdateDto);
    }
}
