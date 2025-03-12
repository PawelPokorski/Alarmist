using System.ComponentModel.DataAnnotations;

namespace Alarmist.API.Models;

public class ResetPasswordViewModel
{
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email is required.")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required.")]
    public string CurrentPassword { get; set; }

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9]).*$", ErrorMessage = "Password must contain at least 1 uppercase letter, 1 digit, and 1 special character.")]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords must match.")]
    public string PasswordRepeat { get; set; }
}