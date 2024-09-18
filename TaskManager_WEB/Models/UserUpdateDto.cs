namespace TaskManager_WEB.Models
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Education { get; set; }
        public string Adress { get; set; }
        public DepartmentDto Department { get; set; } 
    }
}
