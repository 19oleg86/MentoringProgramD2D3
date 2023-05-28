using Categories.Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Categories.Persistence
{
    public class InitialData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context =
                new CategoriesDbContext(serviceProvider.GetRequiredService<DbContextOptions<CategoriesDbContext>>());

            context.Categories.AddRange(
                new Category
                {
                    Id = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "One"
                },
                new Category
                {
                    Id = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Name = "Two"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Three"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Four"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Five"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Six"
                });

            context.Items.AddRange(
                new Item
                {
                    Id = Guid.NewGuid(),
                    CategoryId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Lorem Ipsum"
                },
                new Item
                {
                    Id = Guid.NewGuid(),
                    CategoryId = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    Name = "Nam vel nulla"
                },
                new Item
                {
                    Id = Guid.NewGuid(),
                    CategoryId = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd"),
                    Name = "Pellentesque quam tortor"
                }
            );

            context.SaveChanges();
        }
    }
}