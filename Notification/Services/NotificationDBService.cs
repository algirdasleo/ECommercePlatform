using SharedLibrary.Interfaces;
using SharedLibrary.Services;
using Notification.Models;
using Dapper;
using SharedLibrary.Helpers;

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
            var sql = ResourceHelper.GetQuery("NotificationGetAllAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var notifications = await connection.QueryAsync<NotificationItem>(sql);
                return notifications.ToList();
            }
        }

        public async Task<NotificationItem?> GetByIdAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("NotificationGetByIdAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var notification = await connection.QueryFirstOrDefaultAsync<NotificationItem>(sql, new { id });
                return notification;
            }
        }

        public async Task<NotificationItem> CreateAsync(NotificationItem notification)
        {
            var sql = ResourceHelper.GetQuery("NotificationCreateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var createdNotification = await connection.QueryFirstOrDefaultAsync<NotificationItem>(sql, notification);
                if (createdNotification == null)
                    throw new Exception("Notification not created");
                return createdNotification;
            }
        }

        public async Task<NotificationItem> UpdateAsync(NotificationItem notification)
        {
            var sql = ResourceHelper.GetQuery("NotificationUpdateAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var updatedNotification = await connection.QueryFirstOrDefaultAsync<NotificationItem>(sql, notification);
                if (updatedNotification == null)
                    throw new Exception("Notification not updated");
                return updatedNotification;
            }
        }

        public async Task<NotificationItem> DeleteAsync(int id)
        {
            var sql = ResourceHelper.GetQuery("NotificationDeleteAsync");
            using (var connection = await _dbConnectionFactory.CreateConnectionAsync())
            {
                var deletedNotification = await connection.QueryFirstOrDefaultAsync<NotificationItem>(sql, new { id });
                if (deletedNotification == null)
                    throw new Exception("Notification not deleted");
                return deletedNotification;
            }
        }
    }
}