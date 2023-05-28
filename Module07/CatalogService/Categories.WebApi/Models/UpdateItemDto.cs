using AutoMapper;
using Categories.Application.Common.Mappings;
using Categories.Application.Items.Commands.UpdateItem;

namespace Categories.WebApi.Models;

public class UpdateItemDto : IMapWith<UpdateItemCommand>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CategoryId { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<UpdateItemDto, UpdateItemCommand>()
            .ForMember(xx => xx.Id,
                opt => opt.MapFrom(x => x.Id))
            .ForMember(xx => xx.Name,
                opt => opt.MapFrom(x => x.Name))
            .ForMember(xx => xx.CategoryId,
                opt => opt.MapFrom(x => x.CategoryId));
    }
}
