namespace TaskManager_WEB.Models
{
    public class UserResult
    {
        public UserDto User { get; set; }
        public List<TaskDtoWeb> AssignedTasks { get; set; }
        public List<TaskDtoWeb> CreatedTasks { get; set; }

    }
}


