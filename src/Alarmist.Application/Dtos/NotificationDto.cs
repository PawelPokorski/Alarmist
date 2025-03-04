using Alarmist.Core.Enums;

namespace Alarmist.Application.Dtos;

public class NotificationDto
{
    public NotificationType Type { get; set; }
    public string Details { get; set; }
    public DateTime SendDate { get; set; }
    public int UserId { get; set; }
}

public class UserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}