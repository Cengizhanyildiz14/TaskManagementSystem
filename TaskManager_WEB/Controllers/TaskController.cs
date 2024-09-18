using AutoMapper;
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
        public async Task<IActionResult> TaskDetails(Guid id)
        {
            var response = await _taskService.GetTaskById<APIResponse>(id);
            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess)
            {
                return NotFound();
            }

            var task = JsonConvert.DeserializeObject<TaskDtoWeb>(Convert.ToString(response.result));
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var response = await _taskService.DeleteTask<APIResponse>(id);
            if (response == null)
            {
                return NotFound();
            }

            if (!response.IsSuccess)
            {
                return BadRequest();
            }

            return RedirectToAction("GetAllUsers", "user");
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

            if (!User.IK())
            {
                var ikdepartment = departments.FirstOrDefault(d => d.DepartmentName == "İnsan Kaynakları Uzmanı");
                if (ikdepartment != null)
                {
                    departments = departments.Where(d => d.Id != ikdepartment.Id).ToList();
                }
            }

            var departmentList = new SelectList(departments, "Id", "DepartmentName");

            var usersResponse = await _userService.GetAll<APIResponse>();
            var userResults = usersResponse != null && usersResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(usersResponse.result))
                : new List<UserResult>();

            var allUsers = userResults.Select(ur => ur.User).ToList();

            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var defaultDepartmentId = (Guid)departments.FirstOrDefault()?.Id;
            var filteredUsers = allUsers.Where(u => u.DepartmentId == defaultDepartmentId && u.Id != currentUserId).ToList();

            var taskCreateVM = new TaskCreateVM
            {
                TaskCreateDto = new TaskCreateDto
                {
                    CreaterUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    Status = TaskStatusEnum.Beklemede,
                    DepartmentId = defaultDepartmentId
                },
                DepartmentList = departmentList,
                UserList = new SelectList(filteredUsers, "Id", "Name"),
                AllUsers = allUsers
            };

            return View(taskCreateVM);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskCreateVM taskCreateVM)
        {
            var departmentsResponse = await _departmentService.GetAll<APIResponse>();
            var departments = departmentsResponse != null && departmentsResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(departmentsResponse.result))
                : new List<DepartmentDto>();
            taskCreateVM.DepartmentList = new SelectList(departments, "Id", "DepartmentName");

            var usersResponse = await _userService.GetAll<APIResponse>();
            var userResults = usersResponse != null && usersResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(usersResponse.result))
                : new List<UserResult>();

            var allUsers = userResults.Select(ur => ur.User).ToList();
            taskCreateVM.AllUsers = allUsers;

            if (taskCreateVM.TaskCreateDto.DepartmentId != null)
            {
                var filteredUsers = allUsers.Where(u => u.DepartmentId == taskCreateVM.TaskCreateDto.DepartmentId).ToList();
                taskCreateVM.UserList = new SelectList(filteredUsers, "Id", "Name");
            }
            else
            {
                taskCreateVM.UserList = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            var taskCreateDto = new TaskCreateDto
            {
                TaskName = taskCreateVM.TaskCreateDto.TaskName,
                DepartmentId = taskCreateVM.TaskCreateDto.DepartmentId,
                CreaterUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                AsaignedUserId = taskCreateVM.TaskCreateDto.AsaignedUserId,
                Status = taskCreateVM.TaskCreateDto.Status,
                AssignmentDate = DateTime.Now,
            };

            var response = await _taskService.CreateTask<APIResponse>(taskCreateDto);
            if (response == null || !response.IsSuccess)
            {
                ModelState.AddModelError("", "Görev oluşturulurken bir hata oluştu.");
                return View(taskCreateVM);
            }

            return RedirectToAction("create", "task");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Update(Guid id)
        {

            var response = await _taskService.GetTaskById<APIResponse>(id);
            if (response == null || !response.IsSuccess)
            {
                return NotFound("Görev bulunamadı.");
            }

            var task = JsonConvert.DeserializeObject<TaskDtoWeb>(Convert.ToString(response.result));
            if (task == null)
            {
                return NotFound("Görev detayları yüklenemedi.");
            }

            var departmentsResponse = await _departmentService.GetAll<APIResponse>();
            var departments = departmentsResponse != null && departmentsResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(departmentsResponse.result))
                : new List<DepartmentDto>();
            var departmentList = new SelectList(departments, "Id", "DepartmentName");

            var usersResponse = await _userService.GetAll<APIResponse>();
            var userResults = usersResponse != null && usersResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(usersResponse.result))
                : new List<UserResult>();

            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var userList = userResults.Any()
               ? userResults.Select(ur => ur.User)
                   .Where(u => u.Id != currentUserId)
                   .ToList()
               : new List<UserDto>();

            var taskUpdateVM = new TaskUpdateVM
            {
                TaskUpdateDto = _mapper.Map<TaskUpdateDto>(task),
                DepartmentList = departmentList,
                UserList = new SelectList(userList, "Id", "Name")
            };

            return View(taskUpdateVM);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(TaskUpdateVM taskUpdateVM)
        {


            var departmentsResponse = await _departmentService.GetAll<APIResponse>();
            var departments = departmentsResponse != null && departmentsResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<DepartmentDto>>(Convert.ToString(departmentsResponse.result))
                : new List<DepartmentDto>();
            taskUpdateVM.DepartmentList = new SelectList(departments, "Id", "DepartmentName");

            var usersResponse = await _userService.GetAll<APIResponse>();
            var userResults = usersResponse != null && usersResponse.IsSuccess
                ? JsonConvert.DeserializeObject<List<UserResult>>(Convert.ToString(usersResponse.result))
                : new List<UserResult>();

            var currentUserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userList = userResults.Any()
                ? userResults.Select(ur => ur.User).Where(u => u.Id != currentUserId).ToList()
                : new List<UserDto>();

            taskUpdateVM.UserList = new SelectList(userList, "Id", "Name");

            taskUpdateVM.TaskUpdateDto.AssignmentDate = DateTime.Now;

            var response = await _taskService.UpdateTask<APIResponse>(taskUpdateVM.TaskUpdateDto.Id, taskUpdateVM.TaskUpdateDto);
            if (response == null || !response.IsSuccess)
            {
                ModelState.AddModelError("", "Görev güncellenirken bir hata oluştu.");
                return View(taskUpdateVM);
            }

            return RedirectToAction("GetAllUsers", "user");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TaskStatusUpdate(Guid TaskId, string Status)
        {
            var taskResponse = await _taskService.GetTaskById<APIResponse>(TaskId);
            if (taskResponse == null)
            {
                return NotFound();
            }

            var task = JsonConvert.DeserializeObject<TaskDtoWeb>(Convert.ToString(taskResponse.result));

            if (Status == "Onayla")
            {
                task.Status = (int)TaskStatusEnum.Tamamlandı;
            }
            else if (Status == "Reddet")
            {
                task.Status = (int)TaskStatusEnum.Reddedildi;
            }
            else if (Status == "Süre Doldu")
            {
                task.Status = (int)TaskStatusEnum.SüreDoldu;
            }

            var taskUpdateDto = _mapper.Map<TaskUpdateDto>(task);

            var updateResponse = await _taskService.UpdateTask<APIResponse>(TaskId, taskUpdateDto);
            if (updateResponse == null || !updateResponse.IsSuccess)
            {
                // Güncelleme başarısızsa hata döndür
                return StatusCode(500, "Görev durumu güncellenemedi.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return RedirectToAction("userstask", "user", new { id = userId });
        }

    }
}