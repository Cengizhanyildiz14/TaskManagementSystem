namespace TaskManager_WEB.Services.IServices
{
    public interface IDepartmentService
    {
        Task<T> GetAll<T>();
    }
}
