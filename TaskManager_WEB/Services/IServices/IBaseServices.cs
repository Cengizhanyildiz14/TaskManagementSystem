using TaskManager_WEB.Models;

namespace TaskManager_WEB.Services.IServices
{
    public interface IBaseServices
    {
        APIResponse responseModel { get; set; }
        Task<T> Send<T>(APIRequest apiRequest);
    }
}
