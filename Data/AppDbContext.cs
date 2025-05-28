using Microsoft.EntityFrameworkCore;
using BBTest.Models;

namespace BBTest.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            try
            {
                Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при создании базы данных: " + ex.Message);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.Email)
                      .IsRequired();
                entity.Property(u => u.Balance)
                      .HasPrecision(18, 8);
            });
        }
    }
}
