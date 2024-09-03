namespace TaskManager_WEB.Services.IServices
{
    public interface IAnnouncementService
    {
        Task<T> GetAll<T>();
        Task<T> Delete<T>(int id);
    }
}
