using Categories.Application.Common.Exceptions;
using Categories.Application.Interfaces;
using Categories.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Categories.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler
    : IRequestHandler<UpdateCategoryCommand>
{
    private readonly ICategoriesDbContext _dbContext;

    public UpdateCategoryCommandHandler(ICategoriesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Unit> Handle(UpdateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var entity =
            await _dbContext.Categories.FirstOrDefaultAsync(Category =>
                Category.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }

        entity.Name = request.Name;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
