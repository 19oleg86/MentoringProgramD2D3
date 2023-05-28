using AutoMapper;
using Categories.Application.Common.Mappings;
using Categories.Domain;

namespace Categories.Application.Items.Queries.GetItemDetails;

public class ItemDetailsVm : IMapWith<Item>
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Item, ItemDetailsVm>()
            .ForMember(xx => xx.Name,
                opt => opt.MapFrom(x => x.Name))
            .ForMember(xx => xx.Id,
                opt => opt.MapFrom(x => x.Id))
            .ForMember(xx => xx.CategoryId,
                opt => opt.MapFrom(x => x.CategoryId));
    }
}
