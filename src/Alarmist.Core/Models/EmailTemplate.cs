using Alarmist.Core.Enums;

namespace Alarmist.Core.Models;

public class EmailTemplate
{
    public int Id { get; set; }
    public NotificationType Type { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}