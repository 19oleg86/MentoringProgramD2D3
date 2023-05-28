using System;
using FluentValidation;

namespace Categories.Application.Categories.Queries.GetCategoryDetails;

public class GetCategoryDetailsQueryValidator : AbstractValidator<GetCategoryDetailsQuery>
{
    public GetCategoryDetailsQueryValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
    }
}
