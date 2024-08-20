namespace Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public List<ToDoTask> Tasks { get; set; }
        public List<ToDoTask> CreatedTasks { get; set; }
        public string Gender { get; set; }
    }
}
