using Categories.Application.Interfaces;
using Categories.Domain;
using MediatR;

namespace Categories.Application.Items.Commands.CreateItem;

public class CreateItemCommandHandler
    :IRequestHandler<CreateItemCommand, Guid>
{
    private readonly ICategoriesDbContext _dbContext;

    public CreateItemCommandHandler(ICategoriesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Guid> Handle(CreateItemCommand request,
        CancellationToken cancellationToken)
    {
        var item = new Item
        {
            Name = request.Name,
            Id = Guid.NewGuid(),
            CategoryId = request.CategoryId,
        };

        await _dbContext.Items.AddAsync(item, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return item.Id;
    }
}
