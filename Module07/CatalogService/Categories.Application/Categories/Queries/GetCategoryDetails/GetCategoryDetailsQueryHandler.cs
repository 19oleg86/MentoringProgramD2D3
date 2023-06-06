using AutoMapper;
using Categories.Application.Common.Exceptions;
using Categories.Application.Interfaces;
using Categories.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Categories.Application.Categories.Queries.GetCategoryDetails;

public class GetCategoryDetailsQueryHandler
    : IRequestHandler<GetCategoryDetailsQuery, CategoryDetailsVm>
{
    private readonly ICategoriesDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCategoryDetailsQueryHandler(ICategoriesDbContext dbContext,
        IMapper mapper) => (_dbContext, _mapper) = (dbContext, mapper);

    public async Task<CategoryDetailsVm> Handle(GetCategoryDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Categories
            .FirstOrDefaultAsync(x =>
            x.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Category), request.Id);
        }

        return _mapper.Map<CategoryDetailsVm>(entity);
    }
}
