using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models;

public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email jest wymagany")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format email")]
    public string Email { get; set; }
}