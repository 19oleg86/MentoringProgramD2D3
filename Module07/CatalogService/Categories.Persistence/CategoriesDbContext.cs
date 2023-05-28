using Categories.Application.Interfaces;
using Categories.Domain;
using Microsoft.EntityFrameworkCore;

namespace Categories.Persistence;

public class CategoriesDbContext : DbContext, ICategoriesDbContext
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Item> Items { get; set; }
    public CategoriesDbContext(DbContextOptions<CategoriesDbContext> options)
            : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new CategoryConfiguration());
        base.OnModelCreating(builder);
    }
}
