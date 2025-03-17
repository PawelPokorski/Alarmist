using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models.Account;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
    public string Email { get; set; }
}