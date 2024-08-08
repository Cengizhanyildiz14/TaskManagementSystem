namespace TaskManager_WEB.Models
{
    public class UserViewModel
    {
        public UserDto User { get; set; }
        public List<TaskDto> AssignedTasks { get; set; }
        public List<TaskDto> CreatedTasks { get; set; }
    }
}
