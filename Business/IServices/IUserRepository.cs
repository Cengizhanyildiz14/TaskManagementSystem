﻿using Business.Repository;
using Data.Entities;
using Dto.UserDtos;

namespace Business.IServices
{
    public interface IUserRepository : IRepository<User>
    {
        User UpdateUser(User user);
        User GetUserWithDetails(Guid id);
        List<User> GetAllUserWithDetails();
        LoginResponseDto Login(LoginRequestDto loginRequestDto, bool rememberMe);
        List<ToDoTask> GetUserTask(Guid userId);
    }
}
