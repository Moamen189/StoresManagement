using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class UserProfileUpdate
    {
        [Required, MaxLength(100)]
        public string firstName { get; set; } = "";
        [Required, MaxLength(100)]

        public string lastName { get; set; } = "";
        [Required, EmailAddress, MaxLength(100)]

        public string Email { get; set; } = "";
        [Required, MaxLength(20)]

        public string Phone { get; set; } = "";
        [MaxLength(100)]

        public string Address { get; set; } = "";
    }
}
