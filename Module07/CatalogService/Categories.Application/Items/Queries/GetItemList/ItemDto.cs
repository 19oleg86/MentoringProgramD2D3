using AutoMapper;
using Categories.Application.Common.Mappings;
using Categories.Domain;

namespace Categories.Application.Items.Queries.GetItemList;

public class ItemDto : IMapWith<Item>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Item, ItemDto>()
            .ForMember(x => x.Id,
                opt => opt.MapFrom(xx => xx.Id))
            .ForMember(x => x.CategoryId,
                opt => opt.MapFrom(xx => xx.CategoryId))
            .ForMember(x => x.Name,
                opt => opt.MapFrom(xx => xx.Name));
        profile.CreateMap<PaginationQuery, PaginationFilter>();
    }
}
