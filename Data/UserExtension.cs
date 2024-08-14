using System.Security.Claims;

namespace Data
{
    public static class UserExtension
    {
        public static bool IK(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(d => d.Type == "DepartmentName")?.Value == "İnsan Kaynakları Uzmanı";
        }
    }
}
