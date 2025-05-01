using System.ComponentModel.DataAnnotations;

namespace OVRM.Models
{
    public class Customer
    {
        public int Id { get; set; }
        [Required]
        public string IdentityUserId { get; set; } // Link to ASP.NET Identity user
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
