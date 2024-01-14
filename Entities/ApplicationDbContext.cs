using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.UserID).ValueGeneratedOnAdd();
            });
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contacts");
                entity.Property(e => e.ContactId).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Contact>(entity =>
                {
                    entity.HasOne<User>(u => u.User)
                    .WithMany(c => c.Contacts)
                    .HasForeignKey(p => p.UserID);
                });


            //Seed to Contact
            string contactsJson = System.IO.File.ReadAllText("contacts.json");
            List<Contact> contacts = System.Text.Json.JsonSerializer.Deserialize<List<Contact>>(contactsJson);
            foreach (Contact contact in contacts)
            {
                modelBuilder.Entity<Contact>().HasData(contact);
            }

            //Seed to Users
            string usersJson = System.IO.File.ReadAllText("users.json");
            List<User> users = System.Text.Json.JsonSerializer.Deserialize<List<User>>(usersJson);
            foreach (User user in users)
            {
                modelBuilder.Entity<User>().HasData(user);
            }
        }
    }
}
