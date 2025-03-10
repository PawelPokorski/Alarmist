using Alarmist.API.Models;
using Alarmist.Application.Account.Commands.AddUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Alarmist.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IMediator mediator) : Controller
{
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [SwaggerOperation("Create user")]
    public async Task<IActionResult> Register(UserViewModel viewModel)
    {
        if(!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var command = new AddUserCommand(viewModel.Email, viewModel.Password);

        var result = await mediator.Send(command);

        if (result.IsSuccess)
        {
            return Created();
        }

        return Conflict(result.Errors);
    }
}