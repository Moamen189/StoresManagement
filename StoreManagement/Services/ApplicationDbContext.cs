using Microsoft.EntityFrameworkCore;
using StoreManagement.Models;

namespace StoreManagement.Services
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }

        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<PasswordReset> PasswordResets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> PasswordROrderItemsesets { get; set; }

    }
}
