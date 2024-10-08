﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
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
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;


            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Token'daki kullanıcı ID'si geçersiz");
            }

            var userResponse = await _userService.GetUserWithDetails<APIResponse>(userId);

            if (userResponse != null && userResponse.IsSuccess)
            {
                var user = JsonConvert.DeserializeObject<UserResult>(Convert.ToString(userResponse.result));
                ViewBag.Id = user.User.Id.ToString();
                ViewBag.FullName = $"{user.User.Name} {user.User.LastName}";
                ViewBag.Email = user.User.Email;
                ViewBag.DepartmentName = user.User.Department?.DepartmentName;
                ViewBag.Gender = user.User.Gender;
            }
            else
            {
                return NotFound();
            }

            var allUsersResponse = await _userService.GetAll<APIResponse>();

            if (allUsersResponse == null || !allUsersResponse.IsSuccess)
            {
                return NotFound();
            }

            var users = JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(allUsersResponse.result));
            var viewModel = _mapper.Map<List<UserViewModel>>(users);

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile(Guid id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (id != Guid.Parse(userIdClaim))
            {
                return RedirectToAction("NotFoundPage", "home");
            }

            var response = await _userService.GetUserWithDetails<APIResponse>(id);
            var users= JsonConvert.DeserializeObject<UserResult>(Convert.ToString(response.result));


            if (users == null)
            {
                return NotFound();
            }

            var viewModel = _mapper.Map<ProfileVM>(users.User);

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
        public async Task<IActionResult> UserDelete(Guid id)
        {
            var response = await _userService.Delete<APIResponse>(id);
            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }
            return RedirectToAction("UserList", "user");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UsersTask(Guid id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (id != Guid.Parse(userIdClaim))
            {
                return RedirectToAction("NotFoundPage", "home");
            }

            var response = await _userService.GetUserTasks<APIResponse>(id);
            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }

            var tasks = JsonConvert.DeserializeObject<List<TaskDtoWeb>>(Convert.ToString(response.result));
            return View(tasks);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UsersTaskJson(Guid id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (id != Guid.Parse(userIdClaim))
            {
                return RedirectToAction("NotFoundPage", "home");
            }

            var response = await _userService.GetUserTasks<APIResponse>(id);
            if (response == null || !response.IsSuccess)
            {
                return NotFound();
            }

            var tasks = JsonConvert.DeserializeObject<List<TaskDtoWeb>>(Convert.ToString(response.result));
            var pendingTasks = tasks.Where(t => t.Status == (int)TaskStatusEnum.Beklemede).ToList();

            return Json(pendingTasks);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(Guid id)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (id != Guid.Parse(userIdClaim))
            {
                return RedirectToAction("NotFoundPage", "home");
            }

            var response = await _userService.GetUserWithDetails<APIResponse>(id);

            if (response != null && response.IsSuccess)
            {
                var user = JsonConvert.DeserializeObject<UserResult>(Convert.ToString(response.result));
                var viewModel = _mapper.Map<UserUpdateDto>(user.User);

                var profileUpdateVM = new ProfileUpdateVM
                {
                    userUpdateDto = viewModel
                };

                return View(profileUpdateVM);
            }
            else
            {
                ModelState.AddModelError("", "Kullanıcı bilgileri yüklenirken bir hata oluştu.");
                return RedirectToAction("profile", "user");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(ProfileUpdateVM profileUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(profileUpdateVM);
            }

            var userUpdateDto = new UserUpdateDto
            {
                Id = profileUpdateVM.userUpdateDto.Id,
                Email = profileUpdateVM.userUpdateDto.Email,
                PhoneNumber = profileUpdateVM.userUpdateDto.PhoneNumber,
                Education = profileUpdateVM.userUpdateDto.Education,
                Adress = profileUpdateVM.userUpdateDto.Adress,
            };

            var response = await _userService.PutUser<APIResponse>(userUpdateDto, userUpdateDto.Id);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction("Profile", new { id = userUpdateDto.Id });
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu.");
                return View(profileUpdateVM);
            }
        }


    }
}

