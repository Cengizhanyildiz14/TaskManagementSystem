using Data;

namespace TaskManager_WEB.Models
{
    public class TaskDtoWeb
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public int DepartmentId { get; set; }
        public DepartmentDto Department { get; set; }
        public int AsaignedUserId { get; set; }
        public string AsaignedUserName { get; set; }
        public string AsaignedUserEmail { get; set; } // Atanan kişinin e-posta adresi
        public string CreaterUser { get; set; }
        public int CreaterUserId { get; set; }
        public int Status { get; set; }
        public string StatusDescription => ((TaskStatusEnum)Status).ToString();
    }
}
