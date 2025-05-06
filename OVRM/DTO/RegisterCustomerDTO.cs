using System.ComponentModel.DataAnnotations;

namespace OVRM.DTO
{
    public class RegisterCustomerDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Role { get; set; } = "Customer"; // Default role is Customer
    }
}
