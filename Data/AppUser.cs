using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Data
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}
