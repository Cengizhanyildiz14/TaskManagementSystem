﻿using Dto.DepartmentDtos;

public class TaskDto
{
    public int Id { get; set; }
    public string TaskName { get; set; }
    public int DepartmentId { get; set; }
    public DepartmentDto Department { get; set; }
    public int AsaignedUserId { get; set; }
    public string AsaignedUserName { get; set; }
    public string AsaignedUserLastName { get; set; }
    public string AsaignedUserEmail { get; set; } // Atanan kişinin e-posta adresi
    public string CreaterUserName { get; set; }
    public string CreaterUserLastName { get; set; }
    public string CreaterUserEmail { get; set; } // Atayan kişinin e-posta adresi
    public int CreaterUserId { get; set; }
    public int Status { get; set; }
    public DateTime AssignmentDate { get; set; }
}
