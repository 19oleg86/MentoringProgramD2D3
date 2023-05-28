using Microsoft.EntityFrameworkCore;
using Categories.Domain;

namespace Categories.Application.Interfaces;

public interface ICategoriesDbContext
{
    DbSet<Category> Categories { get; set; }
    DbSet<Item> Items { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
