using Alarmist.Core.Models;

namespace Alarmist.Core.Interfaces;

public interface INotificationRepository
{
    Task<Notification> GetById(int id);
    Task<IEnumerable<Notification>> GetPendingNotifications(DateTime currentTime);
    
    void Add(Notification notification);
    void Update(Notification notification);
}