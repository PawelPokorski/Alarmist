using BC = BCrypt.Net.BCrypt;

namespace Alarmist.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string DisplayName { get; private set; }

    public string VerificationCode { get; private set; }
    public DateTime? VerificationCodeExpiry { get; private set; }
    public bool EmailVerified { get; private set; }
    public DateTime? VerificationCodeResendTimer { get; private set; }

    private User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
        DisplayName = email.Substring(0, email.IndexOf('@'));
        EmailVerified = false;
    }

    public static User Create(string email, string password)
    {
        var passwordHash = BC.HashPassword(password);
     
        return new User(email, passwordHash);
    }
}