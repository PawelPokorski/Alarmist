using System.ComponentModel.DataAnnotations;

namespace Alarmist.Domain.Entities;

public class User : Entity
{
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
}