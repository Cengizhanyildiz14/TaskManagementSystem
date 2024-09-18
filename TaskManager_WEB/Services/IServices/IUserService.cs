using TaskManager_WEB.Models;

namespace TaskManager_WEB.Services.IServices
{
    public interface IUserService
    {
        Task<T> GetAll<T>();
        Task<T> GetUserWithDetails<T>(Guid id);
        Task<T> Delete<T>(Guid id);
        Task<T> PostUser<T>(UserCreateDto userCreateDto);
        Task<T> PutUser<T>(UserUpdateDto userUpdateDto, Guid id);
        Task<T> GetUserTasks<T>(Guid id);
    }
}
