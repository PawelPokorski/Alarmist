using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [MinLength(6, ErrorMessage = "Hasło musi mieć co najmniej 6 znaków")]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}