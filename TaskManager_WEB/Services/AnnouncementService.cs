using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;
using Utility;

namespace TaskManager_WEB.Services
{
    public class AnnouncementService : BaseService, IAnnouncementService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string taskUrl;

        public AnnouncementService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            taskUrl = configuration.GetValue<string>("ServiceUrls:TaskManagementAPI");
        }

        public Task<T> CreateAnnouncement<T>(AnnouncementCreateDto announcementCreateDto)
        {
            return Send<T>(new APIRequest()
            {
                ApiType=SD.ApiType.POST,
                Data = announcementCreateDto,
                Url = taskUrl + "/api/Announcement/PostAnnouncement"
            });
        }

        public Task<T> Delete<T>(Guid id)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = taskUrl + "/api/Announcement/DeleteAnnouncement/" + id
            });
        }

        public Task<T> GetAll<T>()
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = taskUrl + "/api/Announcement/GetAllAnnouncements"
            });
        }

        public Task<T> GetAnnouncementById<T>(Guid id)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = taskUrl + "/api/Announcement/GetAnnouncement/" + id
            });
        }

        public Task<T> UpdateAnnouncement<T>(Guid id, AnnouncementUpdateDto announcementUpdateDto)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = announcementUpdateDto,
                Url = taskUrl + "/api/Announcement/PutAnnouncement/" + id
            });
        }
    }
}
