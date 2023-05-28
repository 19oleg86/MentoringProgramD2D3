using AutoMapper;
using AutoMapper.QueryableExtensions;
using Categories.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Categories.Application.Categories.Queries.GetCategoryList;

public class GetCategoryListQueryHandler
    : IRequestHandler<GetCategoryListQuery, CategoryListVm>
{
    private readonly ICategoriesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCategoryListQueryHandler(ICategoriesDbContext dbContext,
        IMapper mapper) =>
        (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<CategoryListVm> Handle(GetCategoryListQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await _dbContext.Categories
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new CategoryListVm { Categories = categories };
    }
}
