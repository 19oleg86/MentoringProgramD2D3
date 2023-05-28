using MediatR;

namespace Categories.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}
