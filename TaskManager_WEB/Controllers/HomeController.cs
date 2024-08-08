using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
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
            var response = await _userService.GetAll<APIResponse>();

            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }

            // API yanıtındaki 'result' kısmını deserialize ediyoruz
            var users = JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(response.result));

            // Modeli ViewModel'e dönüştürüyoruz
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
