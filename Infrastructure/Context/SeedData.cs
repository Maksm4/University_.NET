using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Context
{
    public class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roles = new[] { "Admin", "Student" };
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (_roleManager != null)
            {
                foreach (var roleName in roles)
                {
                    IdentityRole? role = await _roleManager.FindByNameAsync(roleName);
                    if (role == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }
    }
}