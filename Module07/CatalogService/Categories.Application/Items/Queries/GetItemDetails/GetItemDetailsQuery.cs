using MediatR;

namespace Categories.Application.Items.Queries.GetItemDetails;

public class GetItemDetailsQuery : IRequest<ItemDetailsVm>
{
    public Guid Id { get; set; }
}
