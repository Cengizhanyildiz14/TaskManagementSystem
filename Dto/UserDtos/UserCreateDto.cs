﻿namespace Dto.UserDtos
{
    public class UserCreateDto
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public int DepartmentId { get; set; }
    }
}
