using Business.Repository;
using Data.Entities;

namespace Business.IServices
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        Department UpdatDepartment(Department department);
    }
}
