using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using WebApp.Attributes;
using WebApp.Models;

namespace WebApp.Conventions
{
    public class StudentFilterConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            var hasAttribute = action.Attributes.OfType<AuthorizeAttribute>()
                .Any(a => a.Roles != null && a.Roles.Contains(Role.Student));

            if (hasAttribute)
            {
                action.Filters.Add(new RequirePasswordChangeAttribute());
            }
        }
    }
}