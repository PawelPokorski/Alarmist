using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models;

public class VerifyEmailViewModel
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string Email { get; set; }
}