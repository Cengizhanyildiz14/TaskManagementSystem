using Business.IServices;
using Business.Repository;
using Data.Context;
using Data.Entities;
using Data.UserDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Services
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly TaskManagerContext _context;
        private string secretKey;

        public UserRepository(TaskManagerContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public List<User> GetAllUserWithDetails()
        {

            var users = _context.Users
                 .Include(u => u.Department)
                 .ToList();

            foreach (var user in users)
            {
                _context.Entry(user)
                    .Collection(u => u.Tasks)
                    .Query()
                    .Include(t => t.Department)
                    .Include(t => t.AsaignedUser)
                    .Include(t => t.CreaterUser)
                    .Load();

                _context.Entry(user)
                    .Collection(u => u.CreatedTasks)
                    .Query()
                    .Include(t => t.Department)
                    .Include(t => t.AsaignedUser)
                    .Include(t => t.CreaterUser)
                    .Load();
            }

            return users;
        }

        public User GetUserWithDetails(int id)
        {
            var query = _context.Users
                       .Include(u => u.Department)
                       .Where(u => u.Id == id);

            var user = query.FirstOrDefault();

            if (user != null)
            {
                _context.Entry(user)
                        .Collection(u => u.Tasks)
                        .Query()
                        .Include(t => t.Department)
                        .Include(t => t.AsaignedUser)
                        .Include(t => t.CreaterUser)
                        .Load();

                _context.Entry(user)
                        .Collection(u => u.CreatedTasks)
                        .Query()
                        .Include(t => t.Department)
                        .Include(t => t.AsaignedUser)
                        .Include(t => t.CreaterUser)
                        .Load();
            }

            return user;
        }

        public bool IsUniqueUser(string email)
        {
            throw new NotImplementedException();
        }

        public LoginResponseDto Login(LoginRequestDto loginRequestDto, bool rememberMe)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email.ToLower() == loginRequestDto.Email.ToLower());
            if (user == null)
            {
                return new LoginResponseDto()
                {
                    Token = "",
                    User = null
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = rememberMe ? DateTime.Now.AddDays(1) : DateTime.Now.AddMinutes(15),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                Token = tokenHandler.WriteToken(token),
                User = user,
            };
            return loginResponseDto;
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }
    }
}
