using AutoMapper;
using Business;
using Business.IServices;
using Business.Services;
using Data.Entities;
using Dto.DepartmentDtos;
using Dto.TaskDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace TaskManager_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IToDoTaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;

        public TaskController(IToDoTaskRepository taskRepository, IMapper mapper, IUserRepository userRepository, IDepartmentRepository departmentRepository)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            this._apiResponse = new APIResponse();
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
        }

        [HttpGet("GetAllTasks")]
        public ActionResult<APIResponse> GetAll()
        {
            try
            {
                IEnumerable<ToDoTask> tasks = _taskRepository.Getall();
                if (tasks == null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<List<TaskDto>>(tasks);
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }

        [HttpGet("GetTaskById/{id}")]
        public ActionResult<APIResponse> GetTaskById(Guid id)
        {
            try
            {
                var task = _taskRepository.GetTaskById(id);
                if (task == null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return _apiResponse;
                }
                _apiResponse.Result = _mapper.Map<TaskDto>(task);
                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }

        [HttpDelete("DeleteTask/{id}")]
        [Authorize]
        public ActionResult<APIResponse> DeleteTask(Guid id)
        {
            try
            {
                var task = _taskRepository.Get(x => x.Id == id);
                if (task == null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    return _apiResponse;
                }
                _taskRepository.Delete(task);

                _apiResponse.IsSuccess = true;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }

        [HttpPost("CreateTask")]
        [Authorize]
        public ActionResult<APIResponse> CreateTask([FromBody] TaskCreateDto taskCreateDto)
        {
            try
            {
                if (taskCreateDto == null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _apiResponse.Errors = new List<string> { "Geçersiz görev verisi." };
                    return _apiResponse;
                }

                if (_taskRepository.Get(x => x.TaskName.ToLower() == taskCreateDto.TaskName.ToLower() && x.DepartmentId == taskCreateDto.DepartmentId) != null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _apiResponse.Errors = new List<string> { "Aynı isimde bir görev zaten mevcut." };
                    return _apiResponse;
                }

                var task = _mapper.Map<ToDoTask>(taskCreateDto);

                var assignedUser = _userRepository.Get(x => x.Id == taskCreateDto.AsaignedUserId);
                var creatorUser = _userRepository.Get(x => x.Id == taskCreateDto.CreaterUserId);
                var department = _departmentRepository.Get(x => x.Id == taskCreateDto.DepartmentId);

                var departmentDto = new DepartmentDto
                {
                    Id = department.Id,
                    DepartmentName = department.DepartmentName
                };

                var taskDto = new TaskDto
                {
                    Id = task.Id,
                    TaskName = task.TaskName,
                    DepartmentId = department.Id,
                    Department = departmentDto,
                    Status = (int)task.Status,
                    AssignmentDate = task.AssignmentDate,
                    AsaignedUserId = task.AsaignedUserId,
                    AsaignedUserName = assignedUser?.Name,
                    AsaignedUserEmail = assignedUser.Email,
                    AsaignedUserLastName = assignedUser?.LastName,
                    CreaterUserId = task.CreaterUserId,
                    CreaterUserName = creatorUser?.Name,
                    CreaterUserEmail = creatorUser?.Email,
                    CreaterUserLastName = creatorUser?.LastName
                };
                _taskRepository.Create(task);

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.Created;
                _apiResponse.Result = taskDto; 
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return _apiResponse;
        }



        [HttpPut("PutTask/{id}")]
        [Authorize]
        public ActionResult<APIResponse> PutTask(Guid id, [FromBody] TaskUpdateDto taskUpdateDto)
        {
            try
            {
                if (taskUpdateDto == null || id != taskUpdateDto.Id)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _apiResponse.Errors = new List<string> { "Geçersiz görev verisi." };
                    return _apiResponse;
                }

                var existingTask = _taskRepository.GetTaskById(id);
                if (existingTask == null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
                    _apiResponse.Errors = new List<string> { "görev bulunamadı" };
                    return _apiResponse;
                }

                if (existingTask.CreaterUserId != taskUpdateDto.CreaterUserId)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.Forbidden;
                    _apiResponse.Errors = new List<string> {"Bu görevde değişiklik yapmaya yetkin yok"};
                    return _apiResponse;
                }

                _mapper.Map(taskUpdateDto, existingTask);

                _taskRepository.UpdateTask(existingTask);

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
                _apiResponse.Result = _mapper.Map<TaskDto>(existingTask);
                return _apiResponse;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Errors = new List<string> { ex.ToString() };
                _apiResponse.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return _apiResponse;
        }
    }
}
