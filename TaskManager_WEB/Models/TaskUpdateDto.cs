using Data;

namespace TaskManager_WEB.Models
{
    public class TaskUpdateDto
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public int DepartmentId { get; set; }
        public int CreaterUserId { get; set; }
        public int AsaignedUserId { get; set; }
        public TaskStatusEnum Status { get; set; }
    }
}
