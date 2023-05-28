using Categories.Application.Common.Exceptions;
using Categories.Application.Interfaces;
using Categories.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Categories.Application.Items.Commands.UpdateItem;

public class UpdateItemCommandHandler
    : IRequestHandler<UpdateItemCommand>
{
    private readonly ICategoriesDbContext _dbContext;

    public UpdateItemCommandHandler(ICategoriesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Unit> Handle(UpdateItemCommand request,
        CancellationToken cancellationToken)
    {
        var entity =
            await _dbContext.Items.FirstOrDefaultAsync(Category =>
                Category.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Item), request.Id);
        }

        entity.Name = request.Name;
        entity.CategoryId = request.CategoryId;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
