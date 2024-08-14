using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;
using Utility;

namespace TaskManager_WEB.Services
{
    public class TaskService : BaseService, ITaskService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string taskUrl;

        public TaskService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            taskUrl = configuration.GetValue<string>("ServiceUrls:TaskManagementAPI");
        }

        public Task<T> CreateTask<T>(TaskCreateDto dto)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = taskUrl + "/api/Task/CreateTask/"
            });
        }

        public Task<T> DeleteTask<T>(int id)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = taskUrl + "/api/Task/DeleteTask/" + id
            });
        }

        public Task<T> GetTaskById<T>(int id)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = taskUrl + "/api/Task/GetTaskById/" + id
            });
        }

        public Task<T> UpdateTask<T>(int id, TaskUpdateDto task)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = task,
                Url = taskUrl + "/api/Task/PutTask/" + id
            });
        }
    }
}
