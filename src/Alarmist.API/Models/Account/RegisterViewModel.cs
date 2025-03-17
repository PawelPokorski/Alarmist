using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Hasła nie są zgodne")]
    public string RepeatPassword { get; set; }
}