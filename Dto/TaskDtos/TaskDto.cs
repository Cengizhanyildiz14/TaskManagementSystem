using Dto.DepartmentDtos;

public class TaskDto
{
    public Guid Id { get; set; }
    public string TaskName { get; set; }
    public Guid DepartmentId { get; set; }
    public DepartmentDto Department { get; set; }
    public Guid AsaignedUserId { get; set; }
    public string AsaignedUserName { get; set; }
    public string AsaignedUserLastName { get; set; }
    public string AsaignedUserEmail { get; set; }
    public string CreaterUserName { get; set; }
    public string CreaterUserLastName { get; set; }
    public string CreaterUserEmail { get; set; } 
    public Guid CreaterUserId { get; set; }
    public int Status { get; set; }
    public DateTime AssignmentDate { get; set; }
}
