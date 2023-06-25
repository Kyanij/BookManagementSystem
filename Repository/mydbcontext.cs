using BookManagementSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Repository
{
    public class mydbcontext : DbContext
    {
        public DbSet<Book> books { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<User> User { get; set; }

        public DbSet<AppUser> ApplicationUser { get; set; }


        public mydbcontext(DbContextOptions<mydbcontext> options)
      : base(options)
        {
        }
    }
}
