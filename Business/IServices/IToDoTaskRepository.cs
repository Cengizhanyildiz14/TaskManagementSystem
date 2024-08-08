using Business.Repository;
using Data.Entities;

namespace Business.IServices
{
    public interface IToDoTaskRepository : IRepository<ToDoTask>
    {
        ToDoTask UpdateTask(ToDoTask toDoTask);
        ToDoTask GetTaskById(int id);
    }
}
