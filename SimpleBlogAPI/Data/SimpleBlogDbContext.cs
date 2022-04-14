using Microsoft.EntityFrameworkCore;
using SimpleBlog.Models;
using SimpleBlogAPI.Models;

namespace SimpleBlog
{
    public class SimpleBlogDbContext : DbContext
    {
        public DbSet<BlogPost> BlogPost { get; set; }
        public DbSet<Comment> Comment { get; set; }

        public SimpleBlogDbContext(DbContextOptions<SimpleBlogDbContext> options) : base(options)
        {

        }
    }
}
