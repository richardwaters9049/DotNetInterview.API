using DotNetInterview.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace DotNetInterview.API
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Item> Items => Set<Item>();
        public DbSet<Variation> Variations => Set<Variation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Variations)
                .WithOne()
                .HasForeignKey(v => v.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            SeedData.Load(modelBuilder);
        }
    }
}