using Categories.Application.Common.Exceptions;
using Categories.Application.Interfaces;
using Categories.Domain;
using MediatR;

namespace Categories.Application.Items.Commands.DeleteCommand;

public class DeleteItemCommandHandler
    : IRequestHandler<DeleteItemCommand>
{
    private readonly ICategoriesDbContext _dbContext;

    public DeleteItemCommandHandler(ICategoriesDbContext dbContext) =>
        _dbContext = dbContext;

    public async Task<Unit> Handle(DeleteItemCommand request,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Items
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Item), request.Id);
        }

        _dbContext.Items.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
