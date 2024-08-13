using TaskManager_WEB.Models;

namespace TaskManager_WEB.Services.IServices
{
    public interface IDepartmentService
    {
        Task<T> GetAll<T>();
        Task<T> CreateDepartment<T>(DepartmentCreateDto dto);
    }
}
