using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class ProductDto
    {
        [Required, MaxLength(100)]

        public string Name { get; set; } = "";
        [MaxLength(4000)]

        public string? Description { get; set; }
        [Required,MaxLength(100)]

        public string Category { get; set; } = "";
        [Required,MaxLength(100)]

        public string Brand { get; set; } = "";
        [Required]
        public decimal Price { get; set; }
       

        public IFormFile? ImageFileName { get; set; }
    }
}
