using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;
using Utility;

namespace TaskManager_WEB.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string taskUrl;

        public UserService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            taskUrl = configuration.GetValue<string>("ServiceUrls:TaskManagementAPI");
        }

        public Task<T> Delete<T>(int id)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = taskUrl + "/api/TaskManager/DeleteUser/" + id
            });
        }

        public Task<T> GetAll<T>()
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = taskUrl + "/api/TaskManager/GetAllUsers"
            });
        }

        public Task<T> GetUserWithDetails<T>(int id)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = taskUrl + "api/TaskManager/getuser/" + id
            });
        }

        public Task<T> PostUser<T>(UserCreateDto userCreateDto)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = userCreateDto,
                Url = taskUrl + "/api/TaskManager/postuser"
            });
        }
    }
}
