using Microsoft.AspNetCore.Identity;

namespace Mango.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
