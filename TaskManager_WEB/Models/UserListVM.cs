namespace TaskManager_WEB.Models
{
    public class UserListVM
    {
        public UserDto User { get; set; }
        public int CompletedTasksCount { get; set; }
        public int RejectedTasksCount { get; set; }
        public int ExpiredTasksCount { get; set; }
        public int PendingTasksCount { get; set; }
    }
}
