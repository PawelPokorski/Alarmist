using Alarmist.Application.Account.Commands.AddUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Alarmist.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IMediator mediator) : ControllerBase
{
    
    [HttpPost("register")]
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