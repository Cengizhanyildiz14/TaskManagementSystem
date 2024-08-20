using Dto.DepartmentDtos;

namespace Dto.UserDtos
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

    }
}
