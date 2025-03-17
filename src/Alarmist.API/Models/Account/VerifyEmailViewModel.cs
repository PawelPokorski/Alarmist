using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models.Account;

public class VerifyEmailViewModel
{
    public string Email { get; set; }
    [Required(ErrorMessage = "Kod jest wymagany")]
    public string VerificationCode { get; set; }
}