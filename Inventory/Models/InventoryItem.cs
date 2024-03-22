namespace Inventory.Models
{
    public class InventoryItem
    {
        public int InventoryItemId { get; set; }
        public int ProductId { get; set; } // Foreign key -> ProductService.Models.Product
        public int QuantityAvailable { get; set; }
    }
}