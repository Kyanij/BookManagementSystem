using Microsoft.AspNetCore.Identity;

namespace BookManagementSystem.Models
{
    public class User : IdentityUser
    {
        public override string UserName { get; set; }
        public string Password { get; set; }
       
    }
}
