using System.Collections.Generic;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models.Users;

namespace TeacherWebsiteBackEnd.Data
{
    public interface IUserService
    {
        IEnumerable<User> GetUsers();
        User GetUserById(int id);
        User Login(LoginForm loginForm);
        User Register(RegisterForm registerForm);
        void DeleteUsers();
        bool DeleteUserById(int id);
    }
}
