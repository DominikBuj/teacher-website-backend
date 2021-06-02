#nullable enable
using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Models
{
    public class LoginForm
    {
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string? Username { get; set; }
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string? Password { get; set; }
    }
}
#nullable disable