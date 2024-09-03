using TaskManager_WEB.Models;

namespace TaskManager_WEB.Services.IServices
{
    public interface IAnnouncementService
    {
        Task<T> GetAnnouncementById<T>(int id);
        Task<T> GetAll<T>();
        Task<T> Delete<T>(int id);
        Task<T> UpdateAnnouncement<T>(int id, AnnouncementUpdateDto announcementUpdateDto);
    }
}
