using Microsoft.AspNetCore.SignalR;
using Notification.Hubs;
using Notification.Models;
using SharedLibrary.Interfaces;

namespace Notification.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly IDBService<NotificationItem> _dbService;
        public NotificationService(IHubContext<NotificationHub> hubContext, IDBService<NotificationItem> notificationService)
        {
            _hubContext = hubContext;
            _dbService = notificationService;
        }

        public async Task SendNotificationToUser(string userId, string message)
        {
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
        }

        public async Task SendNotificationToAllUsers(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);
        }

        public async Task<List<NotificationItem>> GetAllAsync()
        {
            return await _dbService.GetAllAsync();
        }

        public async Task<NotificationItem?> GetByIdAsync(int id)
        {
            return await _dbService.GetByIdAsync(id);
        }

        public async Task<NotificationItem> CreateAsync(NotificationItem notification)
        {
            return await _dbService.CreateAsync(notification);
        }

        public async Task<NotificationItem> UpdateAsync(NotificationItem notification)
        {
            return await _dbService.UpdateAsync(notification);
        }

        public async Task<NotificationItem> DeleteAsync(int id)
        {
            return await _dbService.DeleteAsync(id);
        }

        public async Task<NotificationItem> MarkNotificationAsSent(int id)
        {
            var notification = await _dbService.GetByIdAsync(id);
            if (notification == null)
                throw new Exception("Notification not found");
            notification.SentAt = DateTime.Now;
            return await _dbService.UpdateAsync(notification);
        }
    }
}