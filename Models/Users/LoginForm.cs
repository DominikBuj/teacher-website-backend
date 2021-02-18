using System.ComponentModel.DataAnnotations;

namespace TeacherWebsiteBackEnd.Models.Users
{
    public class LoginForm
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
