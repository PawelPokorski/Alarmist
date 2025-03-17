using Alarmist.API.Models.Account;
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
        // Pobierz użytkownika na podstawie adresu email
        var userDto = await mediator.Send(new GetUserByEmailQuery(viewModel.Email));

        // Sprawdź, czy użytkownik istnieje i czy hasło jest poprawne
        if (userDto == null || !userDto.VerifyPassword(viewModel.Password))
        {
            ModelState.AddModelError(string.Empty, "Nieprawidłowy email lub hasło");
            return View(viewModel);
        }

        // Sprawdź, czy adres email użytkownika jest zweryfikowany 
        if (!userDto.IsEmailVerified())
        {
            TempData["UserId"] = userDto.Id;
            return RedirectToAction("VerifyEmail");
        }

        // Utwórz listę roszczeń (claims) dla uwierzytelniania
        var claims = new List<Claim> { new(ClaimTypes.Name, viewModel.Email) };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = viewModel.RememberMe
        };

        // Zaloguj użytkownika za pomocą uwierzytelniania opartego na ciasteczkach
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
        // Sprawdź, czy model jest poprawny (czy walidacja przeszła pomyślnie)
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var command = new AddUserCommand(viewModel.Email, viewModel.Password);
        var result = await mediator.Send(command);

        // Sprawdź, czy operacja dodania użytkownika zakończyła się sukcesem
        if (result.IsSuccess)
        {
            var user = result.ReturnValue as UserDto;

            // Zapisz ID użytkownika w TempData i przekieruj do strony weryfikacji emaila
            TempData["UserId"] = user.Id;
            return RedirectToAction("VerifyEmail");
        }

        return Conflict(result.Errors);
    }

    #endregion

    #region Reset Password

    [HttpGet("account-reset-password/{userGuid}/{recoveryGuid}")]
    public async Task<IActionResult> ResetPassword(Guid userGuid, Guid recoveryGuid)
    {
        // Pobierz użytkownika na podstawie userGuid
        var userDto = await mediator.Send(new GetUserByIdQuery(userGuid));

        if (userDto == null)
        {
            return NotFound("Nie znaleziono użytkownika.");
        }

        // Sprawdź, czy recoveryGuid jest poprawny
        if (userDto.RecoveryId != recoveryGuid)
        {
            return BadRequest("Nieprawidłowy link resetowania hasła.");
        }

        // Utwórz model ResetPasswordViewModel i przekaż go do widoku
        var model = new ResetPasswordViewModel
        {
            Email = userDto.Email,
        };

        return View(model);
    }

    //[HttpPost("account-reset-password/{userGuid}/{recoveryGuid}")]
    //public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model, Guid userGuid, Guid recoveryGuid)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        // Sprawdź, czy userGuid i recoveryGuid są nadal poprawne
    //        var userDto = await mediator.Send(new GetUserByIdQuery(userGuid));

    //        if (userDto == null || userDto.RecoveryId != recoveryGuid)
    //        {
    //            return BadRequest("Nieprawidłowy link resetowania hasła.");
    //        }

    //        // Aktualizuj hasło użytkownika
    //        userDto.Password = model.Password;
    //        userDto.RecoveryId = null; // Usuń recoveryGuid
    //        await mediator.Send(new UpdateUserCommand(userDto));

    //        return RedirectToAction("Login", "Account");
    //    }

    //    return View(model);
    //}

    #endregion

    #region Forgot Password
    [HttpGet("account-forgot-password")]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost("account-forgot-password")]
    public async Task<IActionResult> SendRecoveryMail(ForgotPasswordViewModel viewModel)
    {
        // Pobierz dane użytkownika na podstawie adresu email
        var userDto = await mediator.Send(new GetUserByEmailQuery(viewModel.Email));

        if (userDto != null)
        {
            // Generuj nowy GUID, który będzie służył do resetowania hasła
            userDto.RecoveryId = Guid.NewGuid();
            var link = $"https://localhost:44342/account-reset-password/{userDto.Id}/{userDto.RecoveryId}";

            // Utwórz obiekt MailRequest, który zawiera dane do wysłania emaila
            var mailRequest = new MailRequest
            {
                ToEmail = userDto.Email,
                Subject = "[Alarmist] Utracone hasło",
                Body = $"Kliknij poniższy link, aby ustawić nowe hasło:<br><a href=\"{link}\">{link}</a>."
            };

            // Zaktualizuj dane użytkownika w bazie danych, zapisując RecoveryId i wyślij email z linkiem resetującym hasło
            await mediator.Send(new UpdateUserCommand(userDto));
            await SendEmail(mailRequest);
        }

        // Ustaw status resetowania hasła w ViewData i zwróć widok ForgotPassword
        ViewData["ResetStatus"] = "Success";
        return View("ForgotPassword");
    }

    #endregion

    #region Verify Email

    [HttpGet("account-verification")]
    public async Task<IActionResult> VerifyEmail()
    {
        var userId = TempData.Peek("UserId") as Guid?;

        // Sprawdź, czy UserId istnieje
        if (userId == null)
        {
            return RedirectToAction("Login");
        }

        var userDto = await mediator.Send(new GetUserByIdQuery(userId.Value));

        // Sprawdź, czy użytkownik o podanym UserId istnieje
        if (userDto == null)
        {
            return RedirectToAction("Login");
        }

        // Generuj nowy kod weryfikacyjny i zaktualizuj dane użytkownika w bazie danych
        userDto.GenerateVerificationCode();
        await mediator.Send(new UpdateUserCommand(userDto));

        ViewBag.UserId = userId;

        // Utwórz obiekt MailRequest, który zawiera dane do wysłania emaila z kodem weryfikacyjnym
        var mailRequest = new MailRequest
        {
            ToEmail = userDto.Email,
            Subject = "[Alarmist] Kod weryfikacyjny",
            Body = $"Twój kod weryfikacyjny to: {userDto.VerificationCode}"
        };

        // Wyślij email z kodem weryfikacyjnym i zwróć widok
        await SendEmail(mailRequest);
        return View();
    }

    [HttpPost("account-verification")]
    public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel viewModel, Guid UserId)
    {
        // Sprawdź, czy model jest poprawny (czy walidacja przeszła pomyślnie)
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        // Pobierz dane użytkownika na podstawie UserId
        var userDto = await mediator.Send(new GetUserByIdQuery(UserId));

        // Sprawdź, czy użytkownik o podanym UserId istnieje
        if (userDto == null)
        {
            return RedirectToAction("Login");
        }

        // Sprawdź, czy wprowadzony kod weryfikacyjny jest poprawny
        if (userDto.VerifyCode(viewModel.VerificationCode))
        {
            // Zaktualizuj dane dotyczące użytkownika w bazie danych
            userDto.EmailVerified = true;
            userDto.VerificationCode = null;
            await mediator.Send(new UpdateUserCommand(userDto));

            // Usuń UserId z TempData i przekieruj na stronę użytkownika
            TempData.Remove("UserId");
            return RedirectToAction("Index", "Home");
        }

        // Zwróć widok z błędami walidacji
        ModelState.AddModelError(string.Empty, "Nieprawidłowy kod weryfikacyjny.");
        return View(viewModel);
    }


    #endregion

    #region Logout

    [HttpPost("account-logout")]
    public async Task<IActionResult> Logout()
    {
        // Wyloguj użytkownika i przekieruj na stronę logowania
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    #endregion

    #region Common
    
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
}