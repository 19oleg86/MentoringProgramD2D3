using AutoMapper;
using Categories.Application.Common.Mappings;
using Categories.Domain;

namespace Categories.Application.Categories.Queries.GetCategoryList;

public class CategoryDto : IMapWith<Category>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Guid> ItemIds { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryDto>()
            .ForMember(x => x.Id,
                opt => opt.MapFrom(xx => xx.Id))
            .ForMember(x => x.Name,
                opt => opt.MapFrom(xx => xx.Name))
            .ForMember(dest => dest.ItemIds, opt => opt.MapFrom(src => src.Items.Select(i => i.Id)));
    }
}
