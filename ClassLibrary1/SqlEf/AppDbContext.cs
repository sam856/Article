using MaqaletyCore;
using Microsoft.EntityFrameworkCore;

namespace MaqaletyData.SqlEf
{
    public class AppDbContext : DbContext
    {

        // Constructor to accept the connection string from the configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.UseSqlServer("Server=.;Database=Articles;Integrated Security=SSPI;TrustServerCertificate=True;"); // Replace with actual connection string or move to configuration
            
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<AuthorPost> AuthorPosts { get; set; }

    }
}
