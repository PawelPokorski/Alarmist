using Alarmist.API.Models;
using Alarmist.Application.Account.Commands.AddUser;
using Alarmist.Application.Account.Queries.GetUserByEmail;
using Alarmist.Application.Account.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;

namespace Alarmist.API.Controllers;

public class AccountController(IMediator mediator) : Controller
{
    [Route("account-login")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("account-register")]
    public IActionResult Register()
    {
        return View();
    }

    [Route("account-reset-password")]
    public IActionResult ResetPassword()
    {
        return View();
    }

    [HttpPost("account-register")]
    public async Task<IActionResult> Register(UserViewModel viewModel)
    {
        if(!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var command = new AddUserCommand(viewModel.Email, viewModel.Password);

        var result = await mediator.Send(command);

        if (result.IsSuccess)
        {
            return Created();
        }

        return Conflict(result.Errors);
    }

    [HttpPost("account-login")]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        var command = new GetUserByEmailQuery(viewModel.Email);

        var result = await mediator.Send(command);

        if (result == null || !result.VerifyPassword(viewModel.Password))
        {
            ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło");
            return View(viewModel);
        }

        var claims = new List<Claim> { new(ClaimTypes.Name, viewModel.Email) };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = viewModel.RememberMe
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        //return RedirectToAction("Index", "Home");
        return Ok("Zalogowano pomyślnie");
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
}