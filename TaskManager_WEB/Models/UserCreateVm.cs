using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskManager_WEB.Models
{
    public class UserCreateVm
    {
        public UserCreateVm()
        {
            UserCreateDto = new UserCreateDto();
        }
        public UserCreateDto UserCreateDto { get; set; }
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
    }
}
