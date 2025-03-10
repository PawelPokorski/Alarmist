using BC = BCrypt.Net.BCrypt;

namespace Alarmist.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }

    private User(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
    }

    public static User Create(string email, string password)
    {
        var passwordHash = BC.HashPassword(password);
     
        return new User(email, passwordHash);
    }

    public bool VerifyPassword(string password)
    {
        return BC.Verify(password, PasswordHash);
    }
}