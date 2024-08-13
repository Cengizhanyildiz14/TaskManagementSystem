using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;
using Utility;

namespace TaskManager_WEB.Services
{
    public class DepartmentService : BaseService, IDepartmentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string taskUrl;

        public DepartmentService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            taskUrl = configuration.GetValue<string>("ServiceUrls:TaskManagementAPI");

            //GetAllDepartments
        }

        public Task<T> CreateDepartment<T>(DepartmentCreateDto dto)
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = taskUrl + "/api/Department/PostDepartment"
            });
        }

        public Task<T> GetAll<T>()
        {
            return Send<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = taskUrl + "/api/Department/GetAllDepartments"
            });
        }


    }
}
