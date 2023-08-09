using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Photography.Models;

namespace Photography.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Course> Courses { get; set; }  //tell it what models we're using
        public DbSet<Product> Products { get; set; }  //tell it what models we're using


        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Order>()
                .HasOne(o => o.Cart)
                .WithMany()
                .HasForeignKey(o => o.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
                .HasOne(o => o.Cart)
                .WithMany(o => o.CartItems)
                .HasForeignKey(o => o.CartId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }


        //public DbSet<Photography.Models.Course>? Course { get; set; }
    }
}