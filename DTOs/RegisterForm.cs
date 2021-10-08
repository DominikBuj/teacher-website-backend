using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.DTOs
{
    public class RegisterForm
    {
        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 3)]
        public string? Username { get; set; }
        [Required]
        [StringLength(maximumLength: 32, MinimumLength = 3)]
        public string? Password { get; set; }
        [Required]
        [StringLength(32)]
        public string? Role { get; set; }
    }
}