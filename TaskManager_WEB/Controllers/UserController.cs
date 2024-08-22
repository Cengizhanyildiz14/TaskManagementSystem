using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;
using Utility;

namespace TaskManager_WEB.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, IDepartmentService departmentService)
        {
            _userService = userService;
            _mapper = mapper;
            _departmentService = departmentService;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var token = HttpContext.Request.Cookies["AuthToken"];
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = null;

            if (handler.CanReadToken(token))
            {
                jwtToken = handler.ReadJwtToken(token);
            }

            ViewBag.Id = jwtToken?.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
            ViewBag.FullName = jwtToken?.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
            ViewBag.Email = jwtToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            ViewBag.DepartmentName = jwtToken?.Claims.FirstOrDefault(c => c.Type == "DepartmentName")?.Value;
            ViewBag.Gender = jwtToken?.Claims.FirstOrDefault(c => c.Type == "Gender")?.Value;

            var response = await _userService.GetAll<APIResponse>();

            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }

            var users = JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(response.result));

            var viewModel = _mapper.Map<List<UserViewModel>>(users);

            return View(viewModel);
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile(int id)
        {
            var response = await _userService.GetAll<APIResponse>();
            var users = JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(response.result));

            var user = users.FirstOrDefault(u => u.User.Id == id)?.User;

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<ProfileVM>(user);

            return View(viewModel);
        }



        [HttpGet]
        [Authorize(Policy = ("IK"))]
        public async Task<IActionResult> UserCreate()
        {
            var departmentsResponse = await _departmentService.GetAll<APIResponse>();
            var departments = departmentsResponse != null && departmentsResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(departmentsResponse.result))
                : new List<DepartmentDto>();

            if (!departments.Any())
            {
                ModelState.AddModelError("", "Departman bilgileri yüklenemedi.");
                return View(new UserCreateVm { DepartmentList = new SelectList(Enumerable.Empty<SelectListItem>()) });
            }

            if (!User.IK())
            {
                var ikdepartment = departments.FirstOrDefault(d => d.DepartmentName == "İnsan Kaynakları Uzmanı");
                if (ikdepartment != null)
                {
                    departments = departments.Where(d => d.Id != ikdepartment.Id).ToList();
                }
            }

            var departmentList = new SelectList(departments, "Id", "DepartmentName");

            var userCreateVM = new UserCreateVm
            {
                UserCreateDto = new UserCreateDto(),
                DepartmentList = departmentList
            };

            return View(userCreateVM);
        }

        [HttpPost]
        [Authorize(Policy = ("IK"))]
        public async Task<IActionResult> UserCreate(UserCreateVm userCreateVm)
        {
            if (!ModelState.IsValid)
            {
                var departmentResponse = await _departmentService.GetAll<APIResponse>();
                var departments = departmentResponse != null && departmentResponse.IsSuccess ?
                    JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(departmentResponse.result)) : new List<DepartmentDto>();
                userCreateVm.DepartmentList = new SelectList(departments, "Id", "DepartmentName");
            }

            var userCreateDto = new UserCreateDto
            {
                DepartmentId = userCreateVm.UserCreateDto.DepartmentId,
                Email = userCreateVm.UserCreateDto.Email,
                Name = userCreateVm.UserCreateDto.Name,
                LastName = userCreateVm.UserCreateDto.LastName,
                Gender = userCreateVm.UserCreateDto.Gender,
                Adress = userCreateVm.UserCreateDto.Adress,
                Country = userCreateVm.UserCreateDto.Country,
                State = userCreateVm.UserCreateDto.State,
                PhoneNumber = userCreateVm.UserCreateDto.PhoneNumber,
                Education = userCreateVm.UserCreateDto.Education,
            };

            var response = await _userService.PostUser<APIResponse>(userCreateDto);
            if (response == null || !response.IsSuccess)
            {
                ModelState.AddModelError("", "Görev oluşturulurken bir hata oluştu.");
                return View(userCreateVm);
            }

            return RedirectToAction("usercreate", "user");
        }

        [HttpGet]
        [Authorize(Policy = ("IK"))]
        public async Task<IActionResult> UserList()
        {
            var response = await _userService.GetAll<APIResponse>();
            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }

            var users = JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(response.result));

            var viewModel = users.Select(user => new UserListVM
            {
                User = user.User,
                CompletedTasksCount = user.AssignedTasks.Count(t => t.Status == (int)TaskStatusEnum.Tamamlandı),
                RejectedTasksCount = user.AssignedTasks.Count(t => t.Status == (int)TaskStatusEnum.Reddedildi),
                ExpiredTasksCount = user.AssignedTasks.Count(t => t.Status == (int)TaskStatusEnum.SüreDoldu),
                PendingTasksCount = user.AssignedTasks.Count(t => t.Status == (int)TaskStatusEnum.Beklemede)
            }).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Policy = ("IK"))]
        public async Task<IActionResult> UserDelete(int id)
        {
            var response = await _userService.Delete<APIResponse>(id);
            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }
            return RedirectToAction("UserList", "user");
        }

        [HttpGet]
        public async Task<IActionResult> UsersTask(int id)
        {
            var response = await _userService.GetUserTasks<APIResponse>(id);
            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }

            var tasks = JsonConvert.DeserializeObject<List<TaskDtoWeb>>(Convert.ToString(response.result));
            return View(tasks);
        }

    }
}
