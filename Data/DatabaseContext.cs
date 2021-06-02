using Microsoft.EntityFrameworkCore;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Text> Texts => Set<Text>();
        public DbSet<Post> Posts => Set<Post>();
        public DbSet<Publication> Publications => Set<Publication>();
        public DbSet<Link> Links => Set<Link>();
        public DbSet<Dissertation> Dissertations => Set<Dissertation>();
        public DbSet<File> Files => Set<File>();
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}
