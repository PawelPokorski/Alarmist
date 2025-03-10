using BCrypt.Net;

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
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
     
        return new User(email, passwordHash);
    }
}