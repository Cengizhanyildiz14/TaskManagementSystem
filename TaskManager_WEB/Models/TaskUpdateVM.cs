using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManager_WEB.Models
{
    public class TaskUpdateVM
    {
        public TaskUpdateVM()
        {
            TaskUpdateDto = new TaskUpdateDto();
        }

        public TaskUpdateDto TaskUpdateDto { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
        public IEnumerable<SelectListItem> UserList { get; set; }
    }
}
