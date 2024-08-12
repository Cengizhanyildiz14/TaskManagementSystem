using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.TaskDtos
{
    public class TaskUpdateDto
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public int DepartmentId { get; set; }
        public int CreaterUserId { get; set; }
        public int AsaignedUserId { get; set; }
        public int Status { get; set; }
    }
}
