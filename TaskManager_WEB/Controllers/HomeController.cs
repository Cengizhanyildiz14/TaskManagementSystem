using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;
using Utility;

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
            var token = HttpContext.Session.GetString(SD.SessionToken);
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

            var viewModel = users.Select(user => new UserViewModel
            {
                User = _mapper.Map<UserDto>(user.User),
                AssignedTasks = user.AssignedTasks.Select(t => _mapper.Map<TaskDto>(t)).ToList(),
                CreatedTasks = user.CreatedTasks.Select(t => _mapper.Map<TaskDto>(t)).ToList()
            }).ToList();

            return View(viewModel);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
