using DotNetInterview.API.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotNetInterview.API
{
    public static class SeedData
    {
        // For EF Core Migrations
        public static void Load(ModelBuilder modelBuilder)
        {
            var item1 = new Item
            {
                Id = Guid.Parse("a1b2c3d4-1234-5678-9012-abcdef123456"),
                Reference = "A123",
                Name = "Shorts",
                Price = 35
            };

            var item1Variations = new[]
            {
                new Variation { Id = Guid.NewGuid(), ItemId = item1.Id, Size = "Small", Quantity = 7 },
                new Variation { Id = Guid.NewGuid(), ItemId = item1.Id, Size = "Medium", Quantity = 0 },
                new Variation { Id = Guid.NewGuid(), ItemId = item1.Id, Size = "Large", Quantity = 3 }
            };

            var item2 = new Item
            {
                Id = Guid.Parse("b2c3d4e5-2345-6789-0123-bcdef1234567"),
                Reference = "B456",
                Name = "Tie",
                Price = 15
            };

            var item3 = new Item
            {
                Id = Guid.Parse("c3d4e5f6-3456-7890-1234-cdef12345678"),
                Reference = "C789",
                Name = "Shoes",
                Price = 70
            };

            var item3Variations = new[]
            {
                new Variation { Id = Guid.NewGuid(), ItemId = item3.Id, Size = "9", Quantity = 7 },
                new Variation { Id = Guid.NewGuid(), ItemId = item3.Id, Size = "10", Quantity = 8 }
            };

            modelBuilder.Entity<Item>().HasData(item1, item2, item3);
            modelBuilder.Entity<Variation>().HasData(item1Variations.Concat(item3Variations));
        }

        // For runtime initialization
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<DataContext>();
            
            if (!context.Items.Any())
            {
                context.Database.EnsureCreated();
                
                // Add your seed data using the context directly
                var items = new[]
                {
                    new Item { Id = Guid.Parse("a1b2c3d4-1234-5678-9012-abcdef123456"), 
                              Reference = "A123", Name = "Shorts", Price = 35 },
                    new Item { Id = Guid.Parse("b2c3d4e5-2345-6789-0123-bcdef1234567"), 
                              Reference = "B456", Name = "Tie", Price = 15 },
                    new Item { Id = Guid.Parse("c3d4e5f6-3456-7890-1234-cdef12345678"), 
                              Reference = "C789", Name = "Shoes", Price = 70 }
                };

                var variations = new[]
                {
                    new Variation { Id = Guid.NewGuid(), ItemId = items[0].Id, Size = "Small", Quantity = 7 },
                    new Variation { Id = Guid.NewGuid(), ItemId = items[0].Id, Size = "Medium", Quantity = 0 },
                    new Variation { Id = Guid.NewGuid(), ItemId = items[0].Id, Size = "Large", Quantity = 3 },
                    new Variation { Id = Guid.NewGuid(), ItemId = items[2].Id, Size = "9", Quantity = 7 },
                    new Variation { Id = Guid.NewGuid(), ItemId = items[2].Id, Size = "10", Quantity = 8 }
                };

                context.Items.AddRange(items);
                context.Variations.AddRange(variations);
                context.SaveChanges();
            }
        }
    }
}