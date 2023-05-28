using Categories.Application.Interfaces;
using Categories.Domain;
using MediatR;

namespace Categories.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler
    : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoriesDbContext _dbContext;

    public CreateCategoryCommandHandler(ICategoriesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Guid> Handle(CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            Id = Guid.NewGuid()
        };

        await _dbContext.Categories.AddAsync(category, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
