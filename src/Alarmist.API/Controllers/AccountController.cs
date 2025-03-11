using Alarmist.API.Models;
using Alarmist.Application.Account.Commands.AddUser;
using Alarmist.Application.Account.Queries.GetUserByEmail;
using Alarmist.Application.Account.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Alarmist.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IMediator mediator) : Controller
{
    //[HttpGet]
    //public IActionResult Register()
    //{
    //    return View();
    //}

    [HttpPost("register")]
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

    [HttpPost("login")]
    public async Task<IActionResult> VerifyUser(UserViewModel viewModel)
    {
        var command = new GetUserByEmailQuery(viewModel.Email);

        var result = await mediator.Send(command);

        if (result == null || !result.VerifyPassword(viewModel.Password))
        {
            return BadRequest("Nieprawidłowy login lub hasło");
        }

        return Ok("Pomyślnie zalogowano");
    }

    [HttpGet]
    [SwaggerOperation("Get users")]
    public async Task<IActionResult> Get()
    {
        var command = new GetUsersQuery();

        var result = await mediator.Send(command);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    [HttpGet("{email}")]
    [SwaggerOperation("Get user by email")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        var command = new GetUserByEmailQuery(email);

        var result = await mediator.Send(command);

        if(result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    


    //[HttpPost]
    //[SwaggerOperation("Login user")]
    //public async Task<IActionResult> Login(UserViewModel viewModel)
    //{
    //    var user = await mediator.Send(new GetUserByEmailQuery(viewModel.Email));

    //    if(user == null)
    //    {
    //        return BadRequest("Nieprawidłowy login lub hasło");
    //    }

    //    var isPasswordValid = user.VerifyPassword(viewModel.Password);

    //    if(isPasswordValid)
    //    {
    //        return Ok();
    //    }

    //    return BadRequest("Nieprawidłowy login lub hasło");
    //}
}