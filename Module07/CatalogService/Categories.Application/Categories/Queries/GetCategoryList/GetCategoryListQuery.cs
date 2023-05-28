using System;
using MediatR;

namespace Categories.Application.Categories.Queries.GetCategoryList;

public class GetCategoryListQuery : IRequest<CategoryListVm>
{
}
