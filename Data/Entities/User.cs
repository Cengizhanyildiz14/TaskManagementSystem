﻿namespace Data.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Department Department { get; set; }
        public Guid DepartmentId { get; set; }
        public List<ToDoTask> Tasks { get; set; }
        public List<ToDoTask> CreatedTasks { get; set; }
        public string Gender { get; set; }
        public string Education { get; set; }
        public string Adress { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string PhoneNumber { get; set; }
    }
}
