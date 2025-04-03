using WebApp.Models;

namespace WebApp.Extension
{
    public static class UserExtension
    {
        public static string? GetUserId(this ISession session)
        {
            return session.GetString(SessionData.UserId.ToString()) ?? null;
        }

        public static bool HasDefaultPassword(this ISession session)
        {
            return bool.TryParse(session.GetString(SessionData.HasDefaultPassword.ToString()), out bool result) && result;
        }

        public static string? GetEmail(this ISession session)
        {
            return session.GetString(SessionData.Email.ToString()) ?? null;
        }
    }
}