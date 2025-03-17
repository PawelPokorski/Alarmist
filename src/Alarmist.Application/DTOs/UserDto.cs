using BC = BCrypt.Net.BCrypt;

namespace Alarmist.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string DisplayName { get; set; }

    public string VerificationCode { get; set; }
    public bool EmailVerified { get; set; }

    public Guid RecoveryId { get; set; }

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
    }

    public bool VerifyCode(string code)
    {
        return VerificationCode == code;
    }
}