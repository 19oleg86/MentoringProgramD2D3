using Categories.Application.Common.Exceptions;
using Categories.Application.Interfaces;
using Categories.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Categories.Application.Categories.Commands.DeleteCommand;

public class DeleteCategoryCommandHandler
    : IRequestHandler<DeleteCategoryCommand>
{
    private readonly ICategoriesDbContext _dbContext;

    public DeleteCategoryCommandHandler(ICategoriesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Unit> Handle(DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Categories
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.Id , cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }

        foreach (var item in entity.Items)
            _dbContext.Items.Remove(item);

        _dbContext.Categories.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
