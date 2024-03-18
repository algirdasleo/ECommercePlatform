using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using NotificationService.Models;
using Dapper;

namespace NotificationService.Services
{
    public class NotificationDBService : IDBService<Notification>
    {
        private readonly DBConnectionFactory _dbConnectionFactory;

        public NotificationDBService(DBConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var notifications = await connection.QueryAsync<Notification>("SELECT * FROM notifications");
                return notifications.ToList();
            }
        }

        public async Task<Notification?> GetByIdAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var notification = await connection.QueryFirstOrDefaultAsync<Notification>("SELECT * FROM notifications WHERE notificationId = @id", new { id });
                return notification;
            }
        }

        public async Task<Notification> CreateAsync(Notification notification)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = @"
                    INSERT INTO notifications (NotificationId, UserId, Title, Message, CreatedAt, SentAt)
                    VALUES (@NotificationId, @UserId, @Message, @Title, @CreatedAt, @SentAt)
                    RETURNING *";
                var createdNotification = await connection.QueryFirstOrDefaultAsync<Notification>(sqlQuery, notification);
                if (createdNotification == null)
                    throw new Exception("Notification not created");
                return createdNotification;
            }
        }

        public async Task<Notification> UpdateAsync(Notification notification)
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
                var updatedNotification = await connection.QueryFirstOrDefaultAsync<Notification>(sqlQuery, notification);
                if (updatedNotification == null)
                    throw new Exception("Notification not updated");
                return updatedNotification;
            }
        }

        public async Task<Notification> DeleteAsync(int id)
        {
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var sqlQuery = "DELETE FROM notifications WHERE notificationId = @id";
                var deletedNotification = await connection.QueryFirstOrDefaultAsync<Notification>(sqlQuery, new { id });
                if (deletedNotification == null)
                    throw new Exception("Notification not deleted");
                return deletedNotification;
            }
        }
    }
}