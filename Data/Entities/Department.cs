namespace Data.Entities
{
    public class Department
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }

        public List<User> Users { get; set; }
        public List<ToDoTask> Tasks { get; set; }
    }
}
