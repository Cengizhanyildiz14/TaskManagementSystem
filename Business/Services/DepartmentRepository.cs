using Business.IServices;
using Business.Repository;
using Data.Context;
using Data.Entities;

namespace Business.Services
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly TaskManagerContext _taskManagerContext;

        public DepartmentRepository(TaskManagerContext taskManagerContext) : base(taskManagerContext)
        {
            _taskManagerContext = taskManagerContext;
        }

        public Department UpdatDepartment(Department department)
        {
            _taskManagerContext.Departments.Update(department);
            _taskManagerContext.SaveChanges();
            return department;
        }
    }
}
