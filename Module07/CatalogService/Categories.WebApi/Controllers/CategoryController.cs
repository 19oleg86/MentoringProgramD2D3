using AutoMapper;
using Categories.Application.Categories.Commands.CreateCategory;
using Categories.Application.Categories.Commands.DeleteCommand;
using Categories.Application.Categories.Commands.UpdateCategory;
using Categories.Application.Categories.Queries.GetCategoryDetails;
using Categories.Application.Categories.Queries.GetCategoryList;
using Categories.Application.Common.ErrorResponseDTO;
using Categories.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Categories.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly IMapper _mapper;

        public CategoryController(IMapper mapper) => _mapper = mapper;

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
        public async Task<ActionResult<CategoryListVm>> GetAll()
        {
            var query = new GetCategoryListQuery();
            try 
            {
                throw new NotImplementedException();  // Potentially can be SqlException, depending on database provider used
                var vm = await Mediator.Send(query);
                return Ok(vm);
            }
            catch (Exception ex) 
            {
                var errorResponse = new ErrorResponse
                {
                    Message = "We apologize, but an error occurred while processing your request. Please try again later."
                };
                return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
            }
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
        public async Task<ActionResult<CategoryDetailsVm>> Get(Guid id)
        {
            var query = new GetCategoryDetailsQuery
            {
                Id = id
            };
            var vm = await Mediator.Send(query);

            if (vm == null)
            {
                return NotFound(); // Return 404 Not Found
            }
            return Ok(vm);
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
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCategoryDto createCategoryDto)
        {
            var command = _mapper.Map<CreateCategoryCommand>(createCategoryDto);
            var id = await Mediator.Send(command);
            return Ok(id);
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
            var command = _mapper.Map<UpdateCategoryCommand>(updateCategoryDto);
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
            var command = new DeleteCategoryCommand
            {
                Id = id
            };
            await Mediator.Send(command);
            return NoContent();
        }
    }
}
