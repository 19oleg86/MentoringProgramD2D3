using Categories.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Categories.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = "Catalog";
        services.AddDbContext<CategoriesDbContext>(options =>
        {
            options.UseInMemoryDatabase(connectionString);
        });
        services.AddScoped<ICategoriesDbContext>(provider => provider.GetService<CategoriesDbContext>());
        return services;
    }
    public static IServiceCollection SeedInMemoryDatabase(this IServiceCollection services)
    {
        using var provider = services.BuildServiceProvider();
        InitialData.Initialize(provider);
        return services;
    }
}
