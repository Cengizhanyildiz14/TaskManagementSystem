using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UserDtos
{
    public class LoginRequestDto
    {
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
