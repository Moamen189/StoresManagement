using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class UserDto
    {
        [Required]
        public string FirstName { get; set; } = "";
        [Required(ErrorMessage ="Please Provide your Last Name")]
        public string LastName { get; set; } = "";
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        [Required]
        [MinLength(5 , ErrorMessage ="The Address Should be at least 5 characters")]
        [MaxLength(500 , ErrorMessage = "The Address should be at Max 500 characters")]
        public string Address { get; set; } = "";


    }
}
