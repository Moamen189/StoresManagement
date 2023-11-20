using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; } = "";
        [MaxLength(100)]

        public string LastName { get; set; } = "";
        [MaxLength(100)]

        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        [MaxLength(100)]

        public string Subject { get; set; } = "";
        public string Message { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
