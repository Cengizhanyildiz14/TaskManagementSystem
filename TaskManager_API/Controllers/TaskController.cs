using AutoMapper;
using Business;
using Business.IServices;
using Data.Entities;
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
    }
}
