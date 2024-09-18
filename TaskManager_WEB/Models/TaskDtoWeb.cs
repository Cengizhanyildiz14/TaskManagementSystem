using Utility;

namespace TaskManager_WEB.Models
{
    public class TaskDtoWeb
    {
        public Guid Id { get; set; }
        public string TaskName { get; set; }
        public Guid DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }
        public Guid AsaignedUserId { get; set; }
        public string AsaignedUserName { get; set; }
        public string AsaignedUserLastName { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string AsaignedUserEmail { get; set; } // Atanan kişinin e-posta adresi
        public string CreaterUserName { get; set; }
        public string CreaterUserLastName { get; set; }
        public Guid CreaterUserId { get; set; }
        public int Status { get; set; }
        public string StatusDescription => ((TaskStatusEnum)Status).ToString();
    }
}
