using Utility;

namespace TaskManager_WEB.Models
{
    public class TaskCreateDto
    {
        public string TaskName { get; set; }
        public int DepartmentId { get; set; }
        public int CreaterUserId { get; set; }
        public int AsaignedUserId { get; set; }
        public TaskStatusEnum Status { get; set; }
    }
}
