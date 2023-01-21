using Abb.Euopc.SharedDesks.Domain.Common;

namespace Abb.Euopc.SharedDesks.Domain.Interfaces.Services.Common;

public interface IEmailNotificationService
{
    Task SendNotificationAsync(EmailMessage message);
    Task SendNotificationsAsync(IEnumerable<EmailMessage> messages);
}
