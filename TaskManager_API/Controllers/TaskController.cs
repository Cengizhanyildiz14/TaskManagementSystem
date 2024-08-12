using AutoMapper;
using Business;
using Business.IServices;
using Data;
using Data.Entities;
using Data.TaskDtos;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly IToDoTaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;

        public TaskController(IToDoTaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
            this._apiResponse = new APIResponse();
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
        public ActionResult<APIResponse> GetTaskById(int id)
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
        public ActionResult<APIResponse> DeleteTask(int id)
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
        public ActionResult<APIResponse> CreateTask([FromBody] TaskCreateDto taskCreateDto)
        {
            try
            {
                if (taskCreateDto == null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _apiResponse.Errors = new List<string> { "Invalid task data." };
                    return _apiResponse;
                }

                if (_taskRepository.Get(x => x.TaskName.ToLower() == taskCreateDto.TaskName.ToLower()) != null && _taskRepository.Get(x=>x.DepartmentId==taskCreateDto.DepartmentId)!=null)
                {
                    _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return _apiResponse;
                }

                var task = _mapper.Map<ToDoTask>(taskCreateDto);
                task.Status = (TaskStatusEnum)taskCreateDto.Status;

                _taskRepository.Create(task);

                _apiResponse.IsSuccess = true;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.Created;
                _apiResponse.Result = _mapper.Map<TaskDto>(task);
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
