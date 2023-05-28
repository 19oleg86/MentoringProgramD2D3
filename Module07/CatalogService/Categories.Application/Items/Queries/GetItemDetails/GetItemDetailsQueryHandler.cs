using AutoMapper;
using Categories.Application.Common.Exceptions;
using Categories.Application.Interfaces;
using Categories.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Categories.Application.Items.Queries.GetItemDetails;

public class GetItemDetailsQueryHandler
    : IRequestHandler<GetItemDetailsQuery, ItemDetailsVm>
{
    private readonly ICategoriesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetItemDetailsQueryHandler(ICategoriesDbContext dbContext,
        IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ItemDetailsVm> Handle(GetItemDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Items
            .FirstOrDefaultAsync(x =>
            x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Item), request.Id);
        }

        return _mapper.Map<ItemDetailsVm>(entity);
    }
}
