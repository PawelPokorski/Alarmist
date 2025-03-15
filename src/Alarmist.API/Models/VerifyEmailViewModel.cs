using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models;

public class VerifyEmailViewModel
{
    [Required(ErrorMessage = "Kod jest wymagany")]
    public string VerificationCode { get; set; }
}