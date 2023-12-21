namespace StoreManagement.Models
{
    public class CartItemDto
    {
        public Product product { get; set; } = new Product();

        public int Quantity { get; set; }
    }
}
