using System.ComponentModel.DataAnnotations;

namespace StoreManagement.Models
{
    public class UserProfileDto
    {
        public int Id { get; set; }
      
        public string firstName { get; set; } = "";
 

        public string lastName { get; set; } = "";
      

        public string Email { get; set; } = "";


        public string Phone { get; set; } = "";
        

        public string Address { get; set; } = "";
    

        public string Role { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
