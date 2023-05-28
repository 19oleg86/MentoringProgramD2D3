namespace Categories.Persistence;

public class DbInitializer
{
    public static void Initialize(CategoriesDbContext context)
    {
        context.Database.EnsureCreated();
    }
}
