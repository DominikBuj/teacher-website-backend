using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Models.Users
{
    public class RegisterForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
