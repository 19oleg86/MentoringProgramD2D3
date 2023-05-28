using System;
using MediatR;

namespace Categories.Application.Items.Queries.GetItemList;

public class GetItemListQuery : IRequest<ItemListVm>
{
    public Guid? CategoryId { get; set; }
    public PaginationFilter PaginationFilter { get; set; } = null;
}
