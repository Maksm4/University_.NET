using Infrastructure.Context;
using WebApp.Models;

namespace WebApp.Extension
{
    public static class SessionExtension
    {
        public static bool HasDefaultPassword(this ISession session)
        {
            return bool.TryParse(session.GetString(SessionData.HasDefaultPassword.ToString()), out bool result) && result;
        }

        public static string? GetEmail(this ISession session)
        {
            return session.GetString(SessionData.Email.ToString()) ?? null;
        }
        public static int? GetStudentId(this ISession session)
        {
            return session.GetInt32(SessionData.StudentId.ToString()) ?? null;
        }
    }
}