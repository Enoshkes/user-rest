using Microsoft.EntityFrameworkCore;
using user_api.Models;

namespace user_api.Data
{
    public class ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IConfiguration configuration
    ) : DbContext(options)
    {
        public DbSet<UserModel> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasMany(u => u.Friends)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserFriends"));
        }
    }
}
