using TaskManager_WEB.Models;

namespace TaskManager_WEB.Services.IServices
{
    public interface IAnnouncementService
    {
        Task<T> GetAnnouncementById<T>(Guid id);
        Task<T> GetAll<T>();
        Task<T> Delete<T>(Guid id);
        Task<T> UpdateAnnouncement<T>(Guid id, AnnouncementUpdateDto announcementUpdateDto);
        Task<T> CreateAnnouncement<T>(AnnouncementCreateDto announcementCreateDto);
    }
}
