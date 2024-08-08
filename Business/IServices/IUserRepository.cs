using Business.Repository;
using Data.Entities;
using Data.UserDtos;

namespace Business.IServices
{
    public interface IUserRepository : IRepository<User>
    {
        User UpdateUser(User user);
        User GetUserWithDetails(int id);
        List<User> GetAllUserWithDetails();
        bool IsUniqueUser(string email);
        LoginResponseDto Login(LoginRequestDto loginRequestDto, bool rememberMe);
    }
}
