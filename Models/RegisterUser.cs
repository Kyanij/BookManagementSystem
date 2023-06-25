using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Models
{
    public class RegisterUser
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


    }
}
