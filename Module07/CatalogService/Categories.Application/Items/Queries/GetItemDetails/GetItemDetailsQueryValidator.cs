using FluentValidation;

namespace Categories.Application.Items.Queries.GetItemDetails;

public class GetItemDetailsQueryValidator : AbstractValidator<GetItemDetailsQuery>
{
    public GetItemDetailsQueryValidator()
    {
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
    }
}
