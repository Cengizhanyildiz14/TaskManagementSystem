using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UserDtos
{
    public class LoginResponseDto
    {
        public User User { get; set; }
        public string Token { get; set; }
    }
}
