using AutoMapper;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;
using TaskManager_WEB.Models;
using TaskManager_WEB.Services;
using TaskManager_WEB.Services.IServices;

namespace TaskManager_WEB.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService, IMapper mapper, IDepartmentService departmentService, IUserService userService)
        {
            _taskService = taskService;
            _mapper = mapper;
            _departmentService = departmentService;
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TaskDetails(int id)
        {
            var response = await _taskService.GetTaskById<APIResponse>(id);
            if (response == null)
            {
                return NotFound("Response is null");
            }

            if (!response.IsSuccess)
            {
                return NotFound("API response indicates failure");
            }

            var task = JsonConvert.DeserializeObject<TaskDto>(Convert.ToString(response.result));
            if (task == null)
            {
                return NotFound("Task is null");
            }

            return View(task);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var response = await _taskService.DeleteTask<APIResponse>(id);
            if (response == null)
            {
                return NotFound("Response is null");
            }

            if (!response.IsSuccess)
            {
                return BadRequest("API response indicates failure");
            }

            return RedirectToAction("GetAllUsers", "home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var departmentsResponse = await _departmentService.GetAll<APIResponse>();
            var departments = departmentsResponse != null && departmentsResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(departmentsResponse.result))
                : new List<DepartmentDto>();

            if (!departments.Any())
            {
                ModelState.AddModelError("", "Departman bilgileri yüklenemedi.");
                return View(new TaskCreateVM { DepartmentList = new SelectList(Enumerable.Empty<SelectListItem>()) });
            }

            var departmentList = new SelectList(departments, "Id", "DepartmentName");

            // Tüm kullanıcıları alıyoruz
            var usersResponse = await _userService.GetAll<APIResponse>();
            var userResults = usersResponse != null && usersResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(usersResponse.result))
                : new List<UserResult>();

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userList = userResults.Any()
                ? userResults.Select(ur => ur.User)
                    .Where(u => u.Id != currentUserId) // Bu satırla oturum açmış kullanıcıyı listeden çıkarıyoruz
                    .ToList()
                : new List<UserDto>();

            // VM'yi hazırlayıp, view'a gönderiyoruz
            var taskCreateVM = new TaskCreateVM
            {
                TaskCreateDto = new TaskCreateDto
                {
                    CreaterUserId = currentUserId,
                    Status = TaskStatusEnum.Beklemede
                },
                DepartmentList = departmentList,
                UserList = new SelectList(userList, "Id", "Name")
            };

            return View(taskCreateVM);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskCreateVM taskCreateVM)
        {
            // Eğer form valid değilse, formu tekrar gösteriyoruz.
            if (!ModelState.IsValid)
            {
                // Gerekli listeleri tekrar dolduruyoruz.
                var departmentsResponse = await _departmentService.GetAll<APIResponse>();
                var departments = departmentsResponse != null && departmentsResponse.IsSuccess
                    ? JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(departmentsResponse.result))
                    : new List<DepartmentDto>();
                taskCreateVM.DepartmentList = new SelectList(departments, "Id", "DepartmentName");

                var usersResponse = await _userService.GetAll<APIResponse>();
                var userResults = usersResponse != null && usersResponse.IsSuccess
                    ? JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(usersResponse.result))
                    : new List<UserResult>();

                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userList = userResults.Any()
                    ? userResults.Select(ur => ur.User)
                        .Where(u => u.Id != currentUserId) // Bu satırla oturum açmış kullanıcıyı listeden çıkarıyoruz
                        .ToList()
                    : new List<UserDto>();

                taskCreateVM.UserList = new SelectList(userList, "Id", "Name", "LastName");

    
            }

            // API'ye gönderilecek TaskCreateDto nesnesini oluşturma
            var taskCreateDto = new TaskCreateDto
            {
                TaskName = taskCreateVM.TaskCreateDto.TaskName,
                DepartmentId = taskCreateVM.TaskCreateDto.DepartmentId,
                CreaterUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                AsaignedUserId = taskCreateVM.TaskCreateDto.AsaignedUserId,
                Status = taskCreateVM.TaskCreateDto.Status
            };

            // API'ye istek gönderme
            var response = await _taskService.CreateTask<APIResponse>(taskCreateDto);
            if (response == null || !response.IsSuccess)
            {
                ModelState.AddModelError("", "Görev oluşturulurken bir hata oluştu.");
                return View(taskCreateVM);
            }

            // Başarılı ise başka bir sayfaya yönlendirme yapıyoruz
            return RedirectToAction("GetAllUsers", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            // Mevcut görevi API'den alıyoruz
            var response = await _taskService.GetTaskById<APIResponse>(id);
            if (response == null || !response.IsSuccess)
            {
                return NotFound("Görev bulunamadı.");
            }

            var task = JsonConvert.DeserializeObject<TaskDto>(Convert.ToString(response.result));
            if (task == null)
            {
                return NotFound("Görev detayları yüklenemedi.");
            }

            // Departmanlar listesi hazırlanıyor
            var departmentsResponse = await _departmentService.GetAll<APIResponse>();
            var departments = departmentsResponse != null && departmentsResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(departmentsResponse.result))
                : new List<DepartmentDto>();
            var departmentList = new SelectList(departments, "Id", "DepartmentName");

            // Kullanıcılar listesi hazırlanıyor
            var usersResponse = await _userService.GetAll<APIResponse>();
            var userResults = usersResponse != null && usersResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(usersResponse.result))
                : new List<UserResult>();

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userList = userResults.Any()
               ? userResults.Select(ur => ur.User)
                   .Where(u => u.Id != currentUserId) // Bu satırla oturum açmış kullanıcıyı listeden çıkarıyoruz
                   .ToList()
               : new List<UserDto>();

            // TaskUpdateVM modelini hazırlıyoruz
            var taskUpdateVM = new TaskUpdateVM
            {
                TaskUpdateDto = _mapper.Map<TaskUpdateDto>(task), // DTO'yu modellememizle dolduruyoruz
                DepartmentList = departmentList,
                UserList = new SelectList(userList, "Id", "Name") // Doğru alanlarla dolduruyoruz
            };

            return View(taskUpdateVM);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TaskUpdateVM taskUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                // Gerekli listeleri tekrar dolduruyoruz.
                var departmentsResponse = await _departmentService.GetAll<APIResponse>();
                var departments = departmentsResponse != null && departmentsResponse.IsSuccess
                    ? JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(departmentsResponse.result))
                    : new List<DepartmentDto>();
                taskUpdateVM.DepartmentList = new SelectList(departments, "Id", "DepartmentName");

                var usersResponse = await _userService.GetAll<APIResponse>();
                var userResults = usersResponse != null && usersResponse.IsSuccess
                    ? JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(usersResponse.result))
                    : new List<UserResult>();

                var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userList = userResults.Any()
                    ? userResults.Select(ur => ur.User).Where(u => u.Id != currentUserId).ToList()
                    : new List<UserDto>();

                taskUpdateVM.UserList = new SelectList(userList, "Id", "Name", "LastName");

            }

            // API'ye istek gönderme
            var response = await _taskService.UpdateTask<APIResponse>(taskUpdateVM.TaskUpdateDto.Id, taskUpdateVM.TaskUpdateDto);
            if (response == null || !response.IsSuccess)
            {
                ModelState.AddModelError("", "Görev güncellenirken bir hata oluştu.");
                return View(taskUpdateVM);
            }

            // Başarılı ise başka bir sayfaya yönlendirme yapıyoruz
            return RedirectToAction("GetAllUsers", "Home");
        }


    }
}