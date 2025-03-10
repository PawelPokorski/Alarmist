using Alarmist.Application.Account.Commands.AddUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Alarmist.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IMediator mediator) : ControllerBase
{
    
    [HttpPost]
    [SwaggerOperation("Add user")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(AddUserCommand command)
    {
        var result = await mediator.Send(command);

        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }
}