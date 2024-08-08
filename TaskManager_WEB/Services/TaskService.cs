using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;
using Utility;
using System.Net.Http;
using System.Threading.Tasks;

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

        public Task<T> GetTaskById<T>(int id)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = taskUrl + "/api/Task/GetTaskById/" + id
            });
        }
    }
}
