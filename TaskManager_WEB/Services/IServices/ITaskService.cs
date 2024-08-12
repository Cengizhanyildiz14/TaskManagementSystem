using TaskManager_WEB.Models;

namespace TaskManager_WEB.Services.IServices
{
    public interface ITaskService
    {
        Task<T> GetTaskById<T>(int id);
        Task<T> DeleteTask<T>(int id);
        Task<T> CreateTask<T>(TaskCreateDto dto);
    }
}
