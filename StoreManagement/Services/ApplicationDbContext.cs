using Microsoft.EntityFrameworkCore;

namespace StoreManagement.Services
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
    }
}
