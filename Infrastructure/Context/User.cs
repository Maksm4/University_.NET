using Domain.Models.Aggregate;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Context
{
    public class User : IdentityUser
    {
        public Student? student { get; set; }
        public int? studentId { get; set; }
        public bool defaultpassword { get; set; }
    }
}