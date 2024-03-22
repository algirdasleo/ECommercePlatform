using Microsoft.AspNetCore.SignalR;
using Notification.Hubs;
using Notification.Models;
using SharedLibrary.Interfaces;

namespace Notification.Services
{
    public class NotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IDBService<NotificationItem> _notificationService;

        public NotificationService(IHubContext<NotificationHub> hubContext, IDBService<NotificationItem> notificationService)
        {
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        public async Task SendNotificationToUser(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

        public async Task SendNotificationToAllUsers(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }

        public async Task<List<NotificationItem>> GetAllNotifications()
        {
            return await _notificationService.GetAllAsync();
        }

        public async Task<NotificationItem?> GetNotificationById(int id)
        {
            return await _notificationService.GetByIdAsync(id);
        }

        public async Task<NotificationItem> CreateNotification(NotificationItem notification)
        {
            return await _notificationService.CreateAsync(notification);
        }

        public async Task<NotificationItem> UpdateNotification(NotificationItem notification)
        {
            return await _notificationService.UpdateAsync(notification);
        }

        public async Task<NotificationItem> DeleteNotification(int id)
        {
            return await _notificationService.DeleteAsync(id);
        }

        public async Task<NotificationItem> MarkNotificationAsSent(int id)
        {
            var notification = await _notificationService.GetByIdAsync(id);
            if (notification == null)
                throw new Exception("Notification not found");
            notification.SentAt = DateTime.Now;
            return await _notificationService.UpdateAsync(notification);
        }
    }
}