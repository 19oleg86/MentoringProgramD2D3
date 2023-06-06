using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Categories.WebApi.Controllers
{
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator Mediator;
        protected readonly IMapper Mapper;
        protected BaseController(IMediator mediator, IMapper mapper)
        {
            Mediator = mediator;
            Mapper = mapper;
        }
    }
}
