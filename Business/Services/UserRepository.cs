using AutoMapper;
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
                   .Include(u => u.Tasks) // Kullanıcıya atanmış görevleri getir
                   .ThenInclude(t => t.Department) // Görevlerin departman bilgilerini dahil et
                   .Include(u => u.CreatedTasks) // Kullanıcının yarattığı görevleri getir
                   .ThenInclude(t => t.Department) // Görevlerin departman bilgilerini dahil et
                   .ToList();

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
            // Kullanıcıyı e-posta adresine göre bulma
            var user = _context.Users
                .Include(u => u.Department) // Kullanıcının departman bilgisiyle birlikte çekildiğinden emin olun
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

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim("FullName", $"{user.Name} {user.LastName}"),  // FullName claim'i ekleniyor
            new Claim("DepartmentName", user.Department?.DepartmentName ?? "")  // Departman adı token'a ekleniyor
                }),
                Expires = rememberMe ? DateTime.Now.AddDays(1) : DateTime.Now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Token'ı doğrudan çözümleyip NameIdentifier claim'ini kontrol edelim
            var jwtToken = tokenHandler.ReadJwtToken(tokenString);
            var nameIdentifierClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;


            if (nameIdentifierClaim == null)
            {
                throw new Exception("NameIdentifier claim is missing in the generated token.");
            }

            // AutoMapper kullanarak User nesnesini UserDto'ya dönüştürme
            var userDto = _mapper.Map<UserDto>(user);

            return new LoginResponseDto()
            {
                Token = tokenString,
                User = userDto // Dönüştürülmüş UserDto'yu kullanıyoruz
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
