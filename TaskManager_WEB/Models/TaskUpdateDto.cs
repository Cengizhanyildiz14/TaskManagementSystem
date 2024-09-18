using Utility;

namespace TaskManager_WEB.Models
{
    public class TaskUpdateDto
    {
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid CreaterUserId { get; set; }
        public Guid AsaignedUserId { get; set; }
        public TaskStatusEnum Status { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
