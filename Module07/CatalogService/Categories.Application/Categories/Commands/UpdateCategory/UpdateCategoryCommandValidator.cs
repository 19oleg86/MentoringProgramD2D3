using FluentValidation;

namespace Categories.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(updateCategoryCommand => updateCategoryCommand.Name).NotEmpty().MaximumLength(250);
        RuleFor(updateCategoryCommand => updateCategoryCommand.Id).NotEqual(Guid.Empty);
    }
}
