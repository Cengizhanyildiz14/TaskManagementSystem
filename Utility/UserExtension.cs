using System.Security.Claims;

namespace Utility
{
    public static class UserExtension
    {
        public static bool IK(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(d => d.Type == "DepartmentName")?.Value == "İnsan Kaynakları Uzmanı";
        }

        public static bool IsFemale(this ClaimsPrincipal user)
        {
            return user.Claims.FirstOrDefault(c => c.Type == "Gender")?.Value == "Female";
        }
    }
}
