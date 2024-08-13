using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public static class UserExtension
    {
        public static bool IK(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(d=>d.Type== "DepartmentName")?.Value=="İnsan Kaynakları Uzmanı";
        }
    }
}
