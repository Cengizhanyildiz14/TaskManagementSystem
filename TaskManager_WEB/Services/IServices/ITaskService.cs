namespace TaskManager_WEB.Services.IServices
{
    public interface ITaskService
    {
        Task<T> GetTaskById<T>(int id);
    }
}
