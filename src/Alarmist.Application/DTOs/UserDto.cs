using BC = BCrypt.Net.BCrypt;
namespace Alarmist.Application.DTOs;

public class UserDto
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public bool VerifyPassword(string password)
    {
        return BC.Verify(password, PasswordHash);
    }
}