namespace TaskManager_WEB.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int DepartmentId { get; set; }
        public string Gender { get; set; }
        public DepartmentDto Department { get; set; }
        public string Education { get; set; }
        public string Adress { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string PhoneNumber { get; set; }
    }
}
