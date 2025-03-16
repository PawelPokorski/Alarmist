using Alarmist.API.Models;
using Alarmist.API.Models.Services;
using Alarmist.Application.Account.Commands.AddUser;
using Alarmist.Application.Account.Commands.UpdateUser;
using Alarmist.Application.Account.Queries.GetUserByEmail;
using Alarmist.Application.Account.Queries.GetUserById;
using Alarmist.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Alarmist.API.Controllers;

public class AccountController(IMediator mediator, IMailService mailService) : Controller
{
    #region Login

    [HttpGet("account-login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("account-login")]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        var userDto = await mediator.Send(new GetUserByEmailQuery(viewModel.Email));

        if (userDto == null || !userDto.VerifyPassword(viewModel.Password))
        {
            ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło");
            return View(viewModel);
        }

        if (!userDto.IsEmailVerified())
        {
            TempData["UserId"] = userDto.Id;
            return RedirectToAction("VerifyEmail");
        }

        var claims = new List<Claim> { new(ClaimTypes.Name, viewModel.Email) };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = viewModel.RememberMe
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        return RedirectToAction("Index", "Home");
    }

    #endregion

    #region Register

    [HttpGet("account-register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("account-register")]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var command = new AddUserCommand(viewModel.Email, viewModel.Password);
        var result = await mediator.Send(command);
        
        if (result.IsSuccess)
        {
            var user = result.ReturnValue as UserDto;

            TempData["UserId"] = user.Id;
            return RedirectToAction("VerifyEmail");
        }

        return Conflict(result.Errors);
    }

    #endregion

    #region Reset Password


    #endregion

    #region Forgot Password

    #endregion

    #region Verify Email

    [HttpGet("account-verification")]
    public async Task<IActionResult> VerifyEmail()
    {
        var userId = TempData.Peek("UserId") as Guid?;

        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var userDto = await mediator.Send(new GetUserByIdQuery(userId.Value));

        if (userDto == null)
        {
            return RedirectToAction("Login");
        }

        userDto.GenerateVerificationCode();
        await mediator.Send(new UpdateUserCommand(userDto));

        ViewBag.UserId = userId;

        var mailRequest = new MailRequest
        {
            ToEmail = userDto.Email,
            Subject = "Kod weryfikacyjny do serwisu Alarmist",
            Body = $"Twój kod weryfikacyjny to: {userDto.VerificationCode}"
        };

        await SendEmail(mailRequest);

        return View();
    }

    [HttpPost("account-verification")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel viewModel, Guid UserId)
    {
        if(!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var userDto = await mediator.Send(new GetUserByIdQuery(UserId));

        if(userDto == null)
        {
            return RedirectToAction("Login");
        }

        if (userDto.VerifyCode(viewModel.VerificationCode))
        {
            userDto.EmailVerified = true;
            userDto.VerificationCode = null;
            await mediator.Send(new UpdateUserCommand(userDto));
            TempData.Remove("UserId");
            return RedirectToAction("Login"); // RedirectToAction("Index", "Home")
        }

        ModelState.AddModelError(string.Empty, "Nieprawidłowy kod weryfikacyjny.");
        return View(viewModel);
    }

    private async Task SendEmail(MailRequest mailRequest)
    {
        try
        {
            await mailService.SendEmailAsync(mailRequest);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    #endregion

    #region Logout

    [HttpPost("account-logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    #endregion
}