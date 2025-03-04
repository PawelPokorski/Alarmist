using Alarmist.Core.Enums;
using Alarmist.Core.Models;

namespace Alarmist.Core.Interfaces;

public interface IEmailTemplateRepository
{
    Task<EmailTemplate> GetByType(NotificationType type);
}