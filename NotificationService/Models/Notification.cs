namespace NotificationService.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; } // Foreign key -> UserService.Models.User
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? SentAt { get; set;}
    }
}