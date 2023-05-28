using AutoMapper;
using Categories.Application.Common.Mappings;
using Categories.Domain;

namespace Categories.Application.Categories.Queries.GetCategoryDetails;

public class CategoryDetailsVm : IMapWith<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Guid> ItemIds { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryDetailsVm>()
            .ForMember(xx => xx.Name,
                opt => opt.MapFrom(x => x.Name))
            .ForMember(xx => xx.Id,
                opt => opt.MapFrom(x => x.Id))
            .ForMember(dest => dest.ItemIds, opt => opt.MapFrom(src => src.Items.Select(i => i.Id)));
    }
}
