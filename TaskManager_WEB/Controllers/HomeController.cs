using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;

namespace TaskManager_WEB.Controllers
{
    public class HomeController : Controller
    {

        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public HomeController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            // Kullanıcı bilgilerini token'dan çekmek
            var token = HttpContext.Request.Cookies["AuthToken"];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Token'dan gerekli bilgileri ViewBag ile taşımak
            ViewBag.FullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
            ViewBag.Email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            ViewBag.DepartmentName = jwtToken.Claims.FirstOrDefault(c => c.Type == "DepartmentName")?.Value;

            // API'den kullanıcıları almak
            var response = await _userService.GetAll<APIResponse>();

            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }

            var users = JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(response.result));

            var viewModel = users.Select(user =>
            {
                // Burada doğrudan tasks listesini kullanıyoruz
                var allTasks = user.AssignedTasks ?? new List<TaskDto>();

                return new UserViewModel
                {
                    User = _mapper.Map<UserDto>(user.User),
                    AssignedTasks = allTasks, // Tüm görevler AssignedTasks olarak kullanılıyor
                    CreatedTasks = new List<TaskDto>() // CreatedTasks boş kalabilir veya gerekirse tasks ile doldurulabilir
                };
            }).ToList();

            return View(viewModel);
        }

    }
}
