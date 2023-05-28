using System;
using MediatR;

namespace Categories.Application.Categories.Queries.GetCategoryDetails;

public class GetCategoryDetailsQuery : IRequest<CategoryDetailsVm>
{
    public Guid Id { get; set; }
}
