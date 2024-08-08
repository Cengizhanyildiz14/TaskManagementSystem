using TaskManager_WEB.Models;

namespace TaskManager_WEB.Services.IServices
{
    public interface IAuthService
    {
        Task<T> Login<T>(LoginRequestDto loginRequestDto);
    }
}
