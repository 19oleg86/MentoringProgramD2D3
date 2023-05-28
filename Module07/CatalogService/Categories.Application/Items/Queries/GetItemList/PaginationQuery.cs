namespace Categories.Application.Items.Queries.GetItemList;

public class PaginationQuery
{
	public PaginationQuery()
	{
        PageNumber = 1;
        PageSize = 20;
    }
    public PaginationQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
