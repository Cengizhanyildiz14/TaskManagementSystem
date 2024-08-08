using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TaskManager_WEB.Models;
using TaskManager_WEB.Services.IServices;

namespace TaskManager_WEB.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IMapper _mapper;

        public TaskController(ITaskService taskService, IMapper mapper)
        {
            _taskService = taskService;
            _mapper = mapper;
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
    }
}
