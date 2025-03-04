using Alarmist.Core.Enums;

namespace Alarmist.Core.Models;

public class Notification
{
    public int Id { get; set; }
    public NotificationType Type { get; set; }
    public string Details { get; set; }
    public DateTime SendDate { get; set; }
    
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public NotificationStatus Status { get; set; }
}