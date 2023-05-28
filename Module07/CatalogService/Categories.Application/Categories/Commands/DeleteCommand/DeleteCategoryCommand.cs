using MediatR;

namespace Categories.Application.Categories.Commands.DeleteCommand;

public class DeleteCategoryCommand : IRequest
{
    public Guid Id { get; set; }
}
