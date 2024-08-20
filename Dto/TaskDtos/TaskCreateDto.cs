namespace Dto.TaskDtos
{
    public class TaskCreateDto
    {
        public string TaskName { get; set; }
        public int DepartmentId { get; set; }
        public int CreaterUserId { get; set; }
        public int AsaignedUserId { get; set; }
        public int Status { get; set; }
        public DateTime AssignmentDate { get; set; }
    }
}
