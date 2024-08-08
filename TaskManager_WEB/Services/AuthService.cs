using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;
using Utility;

namespace TaskManager_WEB.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string taskUrl;

        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            taskUrl = configuration.GetValue<string>("ServiceUrls:TaskManagementAPI");
        }

        public Task<T> Login<T>(LoginRequestDto loginRequestDto)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = taskUrl + "/api/TaskManager/login"
            });
        }
    }
}
