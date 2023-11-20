using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class Product
    {
        public int Id { get; set; }
        [MaxLength(100)]

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        [MaxLength(100)]

        public string Category { get; set; } = "";
        [MaxLength(100)]

        public string Brand { get; set; } = "";
        [Precision(16,2)]
        public decimal Price { get; set; }
        [MaxLength(100)]

        public string ImageFileName { get; set;} = string.Empty;

        public DateTime CreatedAt { get; set;} = DateTime.Now;
    }
}
