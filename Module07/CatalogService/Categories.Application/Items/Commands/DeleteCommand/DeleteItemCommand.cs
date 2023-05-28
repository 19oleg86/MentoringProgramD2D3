using MediatR;

namespace Categories.Application.Items.Commands.DeleteCommand;

public class DeleteItemCommand : IRequest
{
    public Guid Id { get; set; }
}
