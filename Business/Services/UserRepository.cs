using AutoMapper;
using Business.IServices;
using Business.Repository;
using Data.Context;
using Data.Entities;
using Dto.UserDtos;
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
        private readonly IMapper _mapper;
        private string secretKey;

        public UserRepository(TaskManagerContext context, IConfiguration configuration, IMapper mapper) : base(context)
        {
            _context = context;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _mapper = mapper;
        }

        public List<User> GetAllUserWithDetails()
        {
            var users = _context.Users
                   .Include(u => u.Department)
                   .Include(u => u.Tasks)
                   .ThenInclude(t => t.Department)
                   .Include(u => u.CreatedTasks)
                   .ThenInclude(t => t.Department)
                   .ToList();

            return users;
        }

        public List<ToDoTask> GetUserTask(Guid userId)
        {
            var tasks = _context.Task.Where(t => t.AsaignedUserId == userId)
                .Include(t => t.Department)
                .Include(t => t.CreaterUser)
                .Include(t => t.AsaignedUser).ToList();
            return tasks;
        }

        public User GetUserWithDetails(Guid id)
        {
            var query = _context.Users
                       .Include(u => u.Department)
                       .Where(u => u.Id == id);

            var user = query.FirstOrDefault();

            if (user != null)
            {
                var users = _context.Users
                    .Include(u => u.Department)
                    .Include(u => u.Tasks)
                    .ThenInclude(t => t.Department)
                    .Include(u => u.CreatedTasks)
                    .ThenInclude(t => t.Department)
                    .ToList();
            }

            return user;
        }

        public LoginResponseDto Login(LoginRequestDto loginRequestDto, bool rememberMe)
        {
            var user = _context.Users
                .Include(u => u.Department)
                .FirstOrDefault(u => u.Email.ToLower() == loginRequestDto.Email.ToLower());

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

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Email, user.Email),
        new Claim("FullName", $"{user.Name} {user.LastName}"),
        new Claim("DepartmentName", user.Department?.DepartmentName ?? ""),
        new Claim("Gender", user.Gender)
    };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = rememberMe ? DateTime.Now.AddDays(1) : DateTime.Now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var userDto = _mapper.Map<UserDto>(user);

            return new LoginResponseDto()
            {
                Token = tokenString,
                User = userDto
            };
        }

        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }
    }
}
