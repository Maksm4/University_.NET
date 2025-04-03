using System.Security.Claims;
using WebApp.Models;

namespace WebApp.Extension
{
    public static class UserExtension
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ExtensionClaimTypes.UserId.ToString()) ?? null;
        }

        public static int? GetStudentId(this ClaimsPrincipal user)
        {
            var studentIdClaim = user.FindFirst(ExtensionClaimTypes.StudentId.ToString());
            return studentIdClaim != null ? int.Parse(studentIdClaim.Value) : null;
        }

        public static bool HasDefaultPassword(this ClaimsPrincipal user)
        {
            var hasDefault = user.FindFirst(ExtensionClaimTypes.DefaultPassword.ToString()); 
            return hasDefault != null && bool.TryParse(hasDefault.Value, out bool result) && result;
        }

        public static string? GetEmail(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Email) ?? null;
        }
    }
}