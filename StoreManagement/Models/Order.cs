using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Precision(16,2)]
        public decimal ShippingFee { get; set; }

        public DateTime CreatedAt { get; set; }
        [MaxLength(100)]
        public string DeliveryMethod { get; set; }
        [MaxLength(30)]
        public string PaymenytMethod { get; set; }
        [MaxLength(30)]

        public string PaymentStatus { get; set; }
        [MaxLength(30)]

        public string OrderStatus { get; set; }
        public User User { get; set; } = null!;

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    }
}
