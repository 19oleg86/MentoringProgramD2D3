using FluentValidation;

namespace Categories.Application.Items.Commands.UpdateItem;

public class UpdateItemCommandValidator : AbstractValidator<UpdateItemCommand>
{
    public UpdateItemCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Id).NotEqual(Guid.Empty);
        RuleFor(x => x.CategoryId).NotEqual(Guid.Empty);
    }
}
