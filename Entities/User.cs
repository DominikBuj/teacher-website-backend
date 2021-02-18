using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TeacherWebsiteBackEnd.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        [JsonIgnore]
        public string PasswordHash { get; set; }
        [Required]
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
