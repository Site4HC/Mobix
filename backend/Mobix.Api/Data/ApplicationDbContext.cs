using Microsoft.EntityFrameworkCore;
using Mobix.Api.Models;

namespace Mobix.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Smartphone> Smartphones { get; set; }
        public DbSet<PriceHistory> PriceHistories { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<Store>()
                .HasIndex(s => s.Name)
                .IsUnique();

            modelBuilder.Entity<PriceHistory>()
                .HasOne(p => p.Smartphone)
                .WithMany(s => s.PriceHistories)
                .HasForeignKey(p => p.SmartphoneId);

            modelBuilder.Entity<PriceHistory>()
                .HasOne(p => p.Store)
                .WithMany(s => s.PriceHistories)
                .HasForeignKey(p => p.StoreId);
            
            modelBuilder.Entity<UserFavorite>()
                .HasKey(uf => uf.Id);
            
            modelBuilder.Entity<UserFavorite>()
                .HasIndex(uf => new { uf.UserId, uf.SmartphoneId })
                .IsUnique();
        }
    }
}