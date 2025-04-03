using Infrastructure.Context;
using WebApp.Models;

namespace WebApp.Extension
{
    public static class UserExtension
    {
        public static bool HasDefaultPassword(this ISession session)
        {
            return bool.TryParse(session.GetString(SessionData.HasDefaultPassword.ToString()), out bool result) && result;
        }

        public static void SetIfDefaultpassword(this ISession session,User user, bool value)
        {
            session.SetString(SessionData.HasDefaultPassword.ToString(), value.ToString());
            user.HasDefaultpassword = value;
        }

        public static string? GetEmail(this ISession session)
        {
            return session.GetString(SessionData.Email.ToString()) ?? null;
        }
    }
}