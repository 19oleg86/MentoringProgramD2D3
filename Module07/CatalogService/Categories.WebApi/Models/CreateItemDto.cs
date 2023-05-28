using AutoMapper;
using Categories.Application.Common.Mappings;
using Categories.Application.Items.Commands.CreateItem;
using System.ComponentModel.DataAnnotations;

namespace Categories.WebApi.Models;

public class CreateItemDto : IMapWith<CreateItemCommand>
{
    [Required]
    public string Name { get; set; }
    public Guid CategoryId { get; init; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateItemDto, CreateItemCommand>()
            .ForMember(x => x.CategoryId,
                opt => opt.MapFrom(xx => xx.CategoryId))
            .ForMember(x => x.Name,
                opt => opt.MapFrom(xx => xx.Name));
    }
}
