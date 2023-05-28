using FluentValidation;

namespace Categories.Application.Categories.Commands.DeleteCommand;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(deleteCategoryCommand => deleteCategoryCommand.Id).NotEqual(Guid.Empty);
    }
}
