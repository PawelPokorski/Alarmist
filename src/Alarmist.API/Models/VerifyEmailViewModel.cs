using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models;

public class VerifyEmailViewModel
{
    [Required]
    public string VerificationCode { get; set; }
}