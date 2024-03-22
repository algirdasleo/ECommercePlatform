using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using Notification.Models;
using Dapper;

namespace Notification.Services
{
    public class NotificationDBService : IDBService<NotificationItem>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public NotificationDBService(DBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<NotificationItem>> GetAllAsync()
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var notifications = await connection.QueryAsync<NotificationItem>("SELECT * FROM notifications");
                return notifications.ToList();
            }
        }

        public async Task<NotificationItem?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var notification = await connection.QueryFirstOrDefaultAsync<NotificationItem>("SELECT * FROM notifications WHERE notificationId = @id", new { id });
                return notification;
            }
        }

        public async Task<NotificationItem> CreateAsync(NotificationItem notification)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    INSERT INTO notifications (UserId, Title, Message, CreatedAt, SentAt)
                    VALUES (@UserId, @Message, @Title, @CreatedAt, @SentAt)
                    RETURNING *";
                var createdNotification = await connection.QueryFirstOrDefaultAsync<NotificationItem>(sqlQuery, notification);
                if (createdNotification == null)
                    throw new Exception("Notification not created");
                return createdNotification;
            }
        }

        public async Task<NotificationItem> UpdateAsync(NotificationItem notification)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    UPDATE notifications
                    SET UserId = @UserId,
                        Title = @Title, Message = @Message, 
                        CreatedAt = @CreatedAt, SentAt = @SentAt
                    WHERE NotificationId = @NotificationId
                    RETURNING *";
                var updatedNotification = await connection.QueryFirstOrDefaultAsync<NotificationItem>(sqlQuery, notification);
                if (updatedNotification == null)
                    throw new Exception("Notification not updated");
                return updatedNotification;
            }
        }

        public async Task<NotificationItem> DeleteAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = "DELETE FROM notifications WHERE notificationId = @id RETURNING *";
                var deletedNotification = await connection.QueryFirstOrDefaultAsync<NotificationItem>(sqlQuery, new { id });
                if (deletedNotification == null)
                    throw new Exception("Notification not deleted");
                return deletedNotification;
            }
        }
    }
}