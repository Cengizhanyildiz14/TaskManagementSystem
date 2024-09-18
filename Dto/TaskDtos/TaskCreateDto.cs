namespace Dto.TaskDtos
{
    public class TaskCreateDto
    {
        public string TaskName { get; set; }
        public Guid DepartmentId { get; set; }
        public Guid CreaterUserId { get; set; }
        public Guid AsaignedUserId { get; set; }
        public int Status { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
