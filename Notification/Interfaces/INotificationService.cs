using SharedLibrary.Interfaces;
using Notification.Models;
namespace Notification.Services
{
    public interface INotificationService : IDBService<NotificationItem>
    {
        Task<NotificationItem> MarkNotificationAsSent(int id);
    }
}
