using AutoMapper;
using Categories.Application.Categories.Commands.CreateCategory;
using Categories.Application.Common.Mappings;
using System.ComponentModel.DataAnnotations;

namespace Categories.WebApi.Models;

public class CreateCategoryDto : IMapWith<CreateCategoryCommand>
{
    [Required]
    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateCategoryDto, CreateCategoryCommand>()
            .ForMember(x => x.Name,
                opt => opt.MapFrom(xx => xx.Name));
    }
}
