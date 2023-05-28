using AutoMapper;
using Categories.Application.Items.Commands.CreateItem;
using Categories.Application.Items.Commands.DeleteCommand;
using Categories.Application.Items.Commands.UpdateItem;
using Categories.Application.Items.Queries.GetItemDetails;
using Categories.Application.Items.Queries.GetItemList;
using Categories.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Categories.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class ItemController : BaseController
    {
        private readonly IMapper _mapper;

        public ItemController(IMapper mapper) => _mapper = mapper;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ItemDto>> GetAll([FromQuery] PaginationQuery paginationQuery, Guid CategoriesId)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var query = new GetItemListQuery
            {
                CategoryId = CategoriesId,
                PaginationFilter = paginationFilter
            };
            var vm = await Mediator.Send(query);
            return Ok(vm.Items);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ItemDetailsVm>> Get(Guid id)
        {
            var query = new GetItemDetailsQuery
            {
                Id = id
            };
            var vm = await Mediator.Send(query);
            return Ok(vm);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateItemDto model)
        {
            var command = _mapper.Map<CreateItemCommand>(model);
            var id = await Mediator.Send(command);
            return Ok(id);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] UpdateItemDto model)
        {
            var command = _mapper.Map<UpdateItemCommand>(model);
            await Mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteItemCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
