using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Hasło jest wymagane")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}