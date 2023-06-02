using AutoMapper;
using Categories.Application.Items.Commands.CreateItem;
using Categories.Application.Items.Commands.DeleteCommand;
using Categories.Application.Items.Commands.UpdateItem;
using Categories.Application.Items.Queries.GetItemDetails;
using Categories.Application.Items.Queries.GetItemList;
using Categories.WebApi.Models;
using Categories.WebApi.Models.Output;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Categories.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ItemController : BaseController
    {
        public ItemController(IMediator mediator, IMapper mapper)
            : base(mediator, mapper)
        {
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<OutputDataModel<IEnumerable<ItemDto>>> GetAll([FromQuery] PaginationQuery paginationQuery, Guid CategoriesId)
        {
            var paginationFilter = Mapper.Map<PaginationFilter>(paginationQuery);
            var vm = await Mediator.Send(new GetItemListQuery
            {
                CategoryId = CategoriesId,
                PaginationFilter = paginationFilter
            });
            return new OutputDataModel<IEnumerable<ItemDto>>(vm.Items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<OutputDataModel<ItemDetailsVm>> Get(Guid id)
        {
            var vm = await Mediator.Send(new GetItemDetailsQuery { Id = id });
            return new OutputDataModel<ItemDetailsVm>(vm);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<OutputDataModel<Guid>> Create([FromBody] CreateItemDto model)
        {
            var command = Mapper.Map<CreateItemCommand>(model);
            var id = await Mediator.Send(command);
        
            return new OutputDataModel<Guid>(id);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateItemDto model)
        {
            var command = Mapper.Map<UpdateItemCommand>(model);
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteItemCommand { Id = id });
            return NoContent();
        }
    }
}
