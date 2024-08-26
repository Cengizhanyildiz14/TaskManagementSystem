using TaskManager_WEB.Models;

namespace TaskManager_WEB.Services.IServices
{
    public interface IUserService
    {
        Task<T> GetAll<T>();
        Task<T> GetUserWithDetails<T>(int id);
        Task<T> Delete<T>(int id);
        Task<T> PostUser<T>(UserCreateDto userCreateDto);
        Task<T> PutUser<T>(UserUpdateDto userUpdateDto, int id);
        Task<T> GetUserTasks<T>(int id);
    }
}
