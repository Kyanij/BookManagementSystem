using BookManagementSystem.Models;

namespace BookManagementSystem.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
