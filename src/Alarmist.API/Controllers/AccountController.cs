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
            var userDto = await mediator.Send(new GetUserByEmailQuery(viewModel.Email));
            userDto.GenerateVerificationCode();
            //await mediator.Send(new UpdateUserCommand(userDto));

            return RedirectToAction("VerifyEmail");
        }

        return Conflict(result.Errors);
    }

    [HttpGet("account-verification")]
    public async Task<IActionResult> VerifyEmail()
    {
        var userDto = await mediator.Send(new GetUserByEmailQuery(User.Identity.Name));
        if (userDto.VerificationCodeExpiry < DateTime.UtcNow && userDto.VerificationCodeResendCooldown < DateTime.UtcNow)
        {
            userDto.GenerateVerificationCode();
            //await mediator.Send(new UpdateUserCommand(userDto));

            // Wyślij e-mail
        }
        return View();
    }

    [HttpPost("account-verification")]
    public async Task<IActionResult> VerifyEmail(string verificationCode)
    {
        var userDto = await mediator.Send(new GetUserByEmailQuery(User.Identity.Name));

        if (userDto.VerifyCode(verificationCode))
        {
            userDto.EmailVerified = true;
            userDto.VerificationCode = null;
            userDto.VerificationCodeExpiry = null;
            //await mediator.Send(new UpdateUserCommand(userDto));
            return RedirectToAction("Login");
        }

        ModelState.AddModelError(string.Empty, "Nieprawidłowy kod weryfikacyjny.");
        return View();
    }

    [HttpPost("account-login")]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        var userDto = await mediator.Send(new GetUserByEmailQuery(viewModel.Email));

        if(userDto == null || !userDto.VerifyPassword(viewModel.Password))
        {
            ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło");
            return View(viewModel);
        }

        if(!userDto.IsEmailVerified())
        {
            return RedirectToAction("VerifyEmail");
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
}