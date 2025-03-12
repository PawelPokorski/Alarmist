using BC = BCrypt.Net.BCrypt;

namespace Alarmist.Application.DTOs;

public class UserDto
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string DisplayName { get; set; }

    public string VerificationCode { get; set; }
    public DateTime? VerificationCodeExpiry { get; set; }
    public bool EmailVerified { get; set; }
    public DateTime? VerificationCodeResendCooldown { get; set; }

    public bool VerifyPassword(string password)
    {
        return BC.Verify(password, PasswordHash);
    }

    public bool IsEmailVerified()
    {
        return EmailVerified;
    }

    public void GenerateVerificationCode()
    {
        VerificationCode = new Random().Next(100000, 999999).ToString();
        VerificationCodeExpiry = DateTime.UtcNow.AddMinutes(15);
        VerificationCodeResendCooldown = DateTime.UtcNow.AddMinutes(1);
    }

    public bool VerifyCode(string code)
    {
        return VerificationCode == code && VerificationCodeExpiry > DateTime.UtcNow;
    }
}