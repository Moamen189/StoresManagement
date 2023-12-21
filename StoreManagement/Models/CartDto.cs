namespace StoreManagement.Models
{
    public class CartDto
    {
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();

        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
