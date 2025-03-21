using BC = BCrypt.Net.BCrypt;

namespace Alarmist.Domain.Entities;

public class User : Entity
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string DisplayName { get; set; }

    public string VerificationCode { get; set; }
    public bool EmailVerified { get; set; }

    public Guid? RecoveryId { get; set; }

    private User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
        DisplayName = email.Substring(0, email.IndexOf('@'));
        EmailVerified = false;
        RecoveryId = null;
    }

    public static User Create(string email, string password)
    {
        var passwordHash = HashPassword(password);
     
        return new User(email, passwordHash);
    }

    public static string HashPassword(string password)
    {
        return BC.HashPassword(password);
    }
}