using AutoMapper;
using AutoMapper.QueryableExtensions;
using Categories.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Categories.Application.Items.Queries.GetItemList;

public class GetItemListQueryHandler
    : IRequestHandler<GetItemListQuery, ItemListVm>
{
    private readonly ICategoriesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetItemListQueryHandler(ICategoriesDbContext dbContext,
        IMapper mapper) =>
        (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<ItemListVm> Handle(GetItemListQuery request,
        CancellationToken cancellationToken)
    {
        if (request.PaginationFilter == null)
        {
            var items = await _dbContext.Items
                .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            if (request.CategoryId != new Guid())
                items = await _dbContext.Items
                    .Where(x => x.CategoryId == request.CategoryId)
                    .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
            
            return new ItemListVm { Items = items };
        }

        var skip = (request.PaginationFilter.PageNumber - 1) * request.PaginationFilter.PageSize;
        var result = await _dbContext.Items
            .Skip(skip)
            .Take(request.PaginationFilter.PageSize)
            .ProjectTo<ItemDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        
        return new ItemListVm { Items = result };

    }
}
