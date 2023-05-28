using MediatR;

namespace Categories.Application.Items.Commands.UpdateItem;

public class UpdateItemCommand : IRequest
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
}
