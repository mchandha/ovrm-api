using System.ComponentModel.DataAnnotations;

namespace OVRM.DTO
{
    public class RegisterAdminDTO
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
