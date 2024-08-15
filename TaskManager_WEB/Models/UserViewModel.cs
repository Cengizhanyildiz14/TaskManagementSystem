namespace TaskManager_WEB.Models
{
    public class UserViewModel
    {
        public UserDto User { get; set; }
        public List<TaskDtoWeb> AssignedTasks { get; set; }
        public List<TaskDtoWeb> CreatedTasks { get; set; }
    }
}
