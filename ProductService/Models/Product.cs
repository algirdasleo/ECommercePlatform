namespace ProductService.Models
{
    public class Product
    {
        public int ProductId { get; set;}
        public string ProductName { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}