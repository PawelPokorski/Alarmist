using Alarmist.API.Models;
using Alarmist.API.Models.Services;
using Alarmist.Application.Account.Commands.AddUser;
using Alarmist.Application.Account.Commands.UpdateUser;
using Alarmist.Application.Account.Queries.GetUserByEmail;
using Alarmist.Application.Account.Queries.GetUserById;
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
            userDto.GenerateVerificationCode();
            await mediator.Send(new UpdateUserCommand(userDto));
            
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

        //return RedirectToAction("Index", "Home");
        return Ok("Zalogowano pomyślnie");
    }

    #endregion

    #region Register

    [HttpGet("account-register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("account-register")]
    public async Task<IActionResult> Register(UserViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var command = new AddUserCommand(viewModel.Email, viewModel.Password);
        var result = await mediator.Send(command);

        if (result.IsSuccess)
        {
            var userDto = await mediator.Send(new GetUserByEmailQuery(viewModel.Email));
            
            userDto.GenerateVerificationCode();
            await mediator.Send(new UpdateUserCommand(userDto));

            TempData["UserId"] = userDto.Id;
            return RedirectToAction("VerifyEmail");
        }

        return Conflict(result.Errors);
    }

    #endregion

    #region Reset Password

    [HttpGet("account-reset-password")]
    public IActionResult ResetPassword()
    {
        return View();
    }

    #endregion

    #region Verify Email

    [HttpGet("account-verification")]
    public async Task<IActionResult> VerifyEmail()
    {
        var userId = TempData.Peek("UserId") as Guid?;
        TempData.Remove("UserId");

        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var userDto = await mediator.Send(new GetUserByIdQuery(userId.Value));

        if (userDto == null)
        {
            return RedirectToAction("Login");
        }

        if (userDto.VerificationCodeExpiry < DateTimeOffset.Now || userDto.VerificationCodeResendCooldown < DateTimeOffset.Now)
        {
            userDto.GenerateVerificationCode();
        }

        ViewBag.UserId = userId;

        var mailRequest = new MailRequest
        {
            ToEmail = userDto.Email,
            Subject = "Kod weryfikacyjny do serwisu Alarmist",
            Body = $"Twój kod weryfikacyjny to: {userDto.VerificationCode}"
        };

        try
        {
            await mailService.SendEmailAsync(mailRequest);
            return View();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
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
            userDto.VerificationCodeExpiry = null;
            userDto.VerificationCodeResendCooldown = null;
            await mediator.Send(new UpdateUserCommand(userDto));
            return RedirectToAction("Login");
        }

        ModelState.AddModelError(string.Empty, "Nieprawidłowy kod weryfikacyjny.");
        return View(viewModel);
    }

    #endregion
}