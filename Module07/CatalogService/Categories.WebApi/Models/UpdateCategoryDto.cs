using AutoMapper;
using Categories.Application.Categories.Commands.UpdateCategory;
using Categories.Application.Common.Mappings;

namespace Categories.WebApi.Models;

public class UpdateCategoryDto : IMapWith<UpdateCategoryCommand>
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateCategoryDto, UpdateCategoryCommand>()
            .ForMember(xx => xx.Id,
                opt => opt.MapFrom(x=> x.Id))
            .ForMember(xx => xx.Name,
                opt => opt.MapFrom(x => x.Name));
    }
}
