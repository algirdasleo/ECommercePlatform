using Microsoft.AspNetCore.Mvc;
using Notification.Models;
using Notification.Services;
using Notification.Hubs;
using SharedLibrary.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Notification.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly IDBService<NotificationItem> _notificationService;
        public NotificationController(IDBService<NotificationItem> notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications(){
            var notifications = await _notificationService.GetAllAsync();
            if (!notifications.Any())
                return NotFound();
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id){
            var notification = await _notificationService.GetByIdAsync(id);
            if (notification == null)
                return NotFound();
            return Ok(notification);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(NotificationItem notification){
            var notificationId = (await _notificationService.CreateAsync(notification)).NotificationId;
            notification.NotificationId = notificationId;
            var actionName = nameof(GetNotificationById);
            var routeValues = new { id = notificationId };
            return CreatedAtAction(actionName, routeValues, notification);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNotification(NotificationItem notification){
            var updatedNotification = await _notificationService.UpdateAsync(notification);
            return Ok(updatedNotification);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id){
            var deletedNotification = await _notificationService.DeleteAsync(id);
            return Ok(deletedNotification);
        }
    }
}