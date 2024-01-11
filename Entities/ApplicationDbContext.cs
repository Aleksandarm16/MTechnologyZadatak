using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet <User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Contact>().ToTable("Contacts");
        }
    }
}
