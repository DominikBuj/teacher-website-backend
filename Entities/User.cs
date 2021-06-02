using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TeacherWebsiteBackEnd.Entities
{
    public enum UserRole
    {
        Teacher,
        Creator,
        Admin
    }

    public class User
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        [StringLength(maximumLength: 20, MinimumLength = 3)]
        public string? Username { get; set; }
        [Required]
        [JsonIgnore]
        public string? PasswordHash { get; set; }
        [Required]
        [Column(TypeName = "varchar(20)")]
        public UserRole? Role { get; set; }
        public string? Token { get; set; }
    }
}
