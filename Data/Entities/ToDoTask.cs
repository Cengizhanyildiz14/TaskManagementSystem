namespace Data.Entities
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public User CreaterUser { get; set; }
        public int CreaterUserId { get; set; }
        public User AsaignedUser { get; set; }
        public int AsaignedUserId { get; set; }
        public TaskStatusEnum Status { get; set; }
    }
}
