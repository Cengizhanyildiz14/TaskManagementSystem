using Business.IServices;
using Business.Repository;
using Data.Context;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class ToDoTaskRepository : Repository<ToDoTask>, IToDoTaskRepository
    {
        private readonly TaskManagerContext _taskManagerContext;

        public ToDoTaskRepository(TaskManagerContext taskManagerContext) : base(taskManagerContext)
        {
            _taskManagerContext = taskManagerContext;
        }

        public ToDoTask GetTaskById(int id)
        {
            return _taskManagerContext.Task
                .Include(t => t.Department)
                .Include(t => t.AsaignedUser)
                .Include(t => t.CreaterUser)
                .FirstOrDefault(t => t.Id == id);
        }

        public ToDoTask UpdateTask(ToDoTask toDoTask)
        {
            _taskManagerContext.Task.Update(toDoTask);
            _taskManagerContext.SaveChanges();
            return toDoTask;
        }
    }
}
