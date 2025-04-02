namespace WebApp.externalServices
{
    public class PasswordGenerator : IPasswordGenerator
    {
        public string GenerateRandom()
        {
            return "Password123$";
        }
    }
}