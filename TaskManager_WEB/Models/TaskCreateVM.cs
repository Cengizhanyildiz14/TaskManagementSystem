using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManager_WEB.Models
{
    public class TaskCreateVM
    {

        public TaskCreateVM()
        {
            TaskCreateDto = new TaskCreateDto();
        }

        public TaskCreateDto TaskCreateDto { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }
        public List<UserDto> AllUsers { get; set; }
    }
}
