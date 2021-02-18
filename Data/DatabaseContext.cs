using Microsoft.EntityFrameworkCore;
using TeacherWebsiteBackEnd.Entities;

namespace TeacherWebsiteBackEnd.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<Update> Updates { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    }
}
