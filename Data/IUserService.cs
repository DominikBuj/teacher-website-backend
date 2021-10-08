using System.Collections.Generic;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.DTOs;

namespace TeacherWebsiteBackEnd.Data
{
    public interface IUserService
    {
        Task<bool> UserExistsByUsername(string username);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserById(int id);
        Task<User> Login(LoginForm loginForm);
        Task<User> Register(RegisterForm registerForm);
        void DeleteUsers();
        Task<bool> DeleteUserById(int id);
    }
}
