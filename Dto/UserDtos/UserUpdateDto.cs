using Dto.DepartmentDtos;

namespace Dto.UserDtos
{
    public class UserUpdateDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Education { get; set; }
        public string Adress { get; set; }
    }
}
