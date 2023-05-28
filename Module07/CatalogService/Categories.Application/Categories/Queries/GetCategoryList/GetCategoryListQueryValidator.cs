using System;
using FluentValidation;

namespace Categories.Application.Categories.Queries.GetCategoryList;

public class GetCategoryListQueryValidator : AbstractValidator<GetCategoryListQuery>
{
    public GetCategoryListQueryValidator()
    {
    }
}
