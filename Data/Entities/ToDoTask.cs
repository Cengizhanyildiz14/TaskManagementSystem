using Utility;

namespace Data.Entities
{
    public class ToDoTask
    {
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public Department Department { get; set; }
        public Guid DepartmentId { get; set; }
        public User CreaterUser { get; set; }
        public Guid CreaterUserId { get; set; }
        public User AsaignedUser { get; set; }
        public Guid AsaignedUserId { get; set; }
        public TaskStatusEnum Status { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
