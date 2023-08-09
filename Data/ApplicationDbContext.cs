using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Photography.Models;

namespace Photography.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Course> Courses { get; set; }  //tell it what models we're using
        public DbSet<Product> Products { get; set; }  //tell it what models we're using

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Photography.Models.Course>? Course { get; set; }
    }
}