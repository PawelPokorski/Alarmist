using BC = BCrypt.Net.BCrypt;

namespace Alarmist.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string DisplayName { get; set; }

    public string VerificationCode { get; set; }
    public DateTimeOffset? VerificationCodeExpiry { get; set; }
    public bool EmailVerified { get; set; }
    public DateTimeOffset? VerificationCodeResendCooldown { get; set; }

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
        VerificationCodeExpiry = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddMinutes(15), TimeZoneInfo.Local);
        VerificationCodeResendCooldown = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddMinutes(1), TimeZoneInfo.Local);
    }

    public bool VerifyCode(string code)
    {
        return VerificationCode == code && VerificationCodeExpiry > DateTime.UtcNow;
    }
}