using MaqaletyCore;
using Microsoft.EntityFrameworkCore;

namespace MaqaletyData.SqlEf
{
    public class AppDbContext : DbContext
    {

        // Constructor to accept the connection string from the configuration
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
                optionsBuilder.UseSqlServer("Server=SQL9001.site4now.net;Database=db_aaeb2d_articles;User Id=db_aaeb2d_articles_admin;Password=Menna123@#;timeout=120"); // Replace with actual connection string or move to configuration
            
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<AuthorPost> AuthorPosts { get; set; }

    }
}
