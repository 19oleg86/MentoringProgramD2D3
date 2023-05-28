using MediatR;

namespace Categories.Application.Items.Commands.CreateItem;

public class CreateItemCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public Guid CategoryId { get; init; }
}
