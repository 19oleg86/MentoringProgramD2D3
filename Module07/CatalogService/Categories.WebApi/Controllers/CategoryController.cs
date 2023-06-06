using AutoMapper;
using Categories.Application.Categories.Commands.CreateCategory;
using Categories.Application.Categories.Commands.DeleteCommand;
using Categories.Application.Categories.Commands.UpdateCategory;
using Categories.Application.Categories.Queries.GetCategoryDetails;
using Categories.Application.Categories.Queries.GetCategoryList;
using Categories.WebApi.Models;
using Categories.WebApi.Models.Output;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Categories.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        public CategoryController(IMediator mediator, IMapper mapper)
            :base(mediator, mapper)
        {
        }

        /// <summary>
        /// Gets the list 
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /
        /// </remarks>
        /// <returns>Returns ListVm</returns>
        /// <response code="200">Success</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<OutputDataModel<CategoryListVm>> GetAll()
        {
            var vm = await Mediator.Send(new GetCategoryListQuery());
            return new OutputDataModel<CategoryListVm>(vm);
        }

        /// <summary>
        /// Gets by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /A12B345C-DEF6-789E-12F3-456G789HD580
        /// </remarks>
        /// <param name="id"> id (guid)</param>
        /// <returns>Returns DetailsVm</returns>
        /// <response code="200">Success</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<OutputDataModel<CategoryDetailsVm>> Get(Guid id)
        {
            var vm = await Mediator.Send(new GetCategoryDetailsQuery { Id = id });
            return new OutputDataModel<CategoryDetailsVm>(vm);
        }

        /// <summary>
        /// Creates
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// POST /
        /// {
        ///     name: "name"
        /// }
        /// </remarks>
        /// <param name="createCategoryDto">CreateCategoryDto object</param>
        /// <returns>Returns id (guid)</returns>
        /// <response code="201">Success</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<OutputDataModel<Guid>> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            var command = Mapper.Map<CreateCategoryCommand>(createCategoryDto);
            var id = await Mediator.Send(command);
            return new OutputDataModel<Guid>(id);
        }

        /// <summary>
        /// Updates
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// PUT /
        /// {
        ///     title: "updated"
        /// }
        /// </remarks>
        /// <param name="updateCategoryDto">UpdateCategoryDto object</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryDto updateCategoryDto)
        {
            var command = Mapper.Map<UpdateCategoryCommand>(updateCategoryDto);
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes the by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// DELETE /A12B345C-DEF6-789E-12F3-456G789HD580
        /// </remarks>
        /// <param name="id">Id of the (guid)</param>
        /// <returns>Returns NoContent</returns>
        /// <response code="204">Success</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteCategoryCommand { Id = id });
            return NoContent();
        }
    }
}
