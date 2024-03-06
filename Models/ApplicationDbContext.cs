using Microsoft.EntityFrameworkCore;

namespace ExpanceTracker.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options):base(options) 
        {
            
        }



        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> categories { get; set; }

    }


}
