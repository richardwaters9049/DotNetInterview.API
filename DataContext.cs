using DotNetInterview.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace DotNetInterview.API
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        // Add these DbSets for your entities
        public DbSet<Item> Items { get; set; }
        public DbSet<Variation> Variations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity relationships
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Variations)
                .WithOne(v => v.Item)
                .HasForeignKey(v => v.ItemId);

            // Seed initial data
            SeedData.Load(modelBuilder);
        }
    }
}