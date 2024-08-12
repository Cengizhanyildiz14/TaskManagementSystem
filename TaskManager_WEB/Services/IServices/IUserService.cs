namespace TaskManager_WEB.Services.IServices
{
    public interface IUserService
    {
        Task<T> GetAll<T>();
        Task<T> GetUserWithDetails<T>(int id);
        Task<T> Delete<T>(int id);
    }
}
