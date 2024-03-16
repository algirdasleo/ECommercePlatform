namespace OrderService.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; } // Foreign key -> UserService.Models.User
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public Status OrderStatus { get; set; } = Status.Pending;
    }
}