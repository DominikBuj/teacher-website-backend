using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models.Users;

namespace TeacherWebsiteBackEnd.Data
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public UserService(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }

        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id);
        }

        private bool CorrectPassword(string password, string passwordHash)
        {
            try
            {
                byte[] passwordHashBytes = Convert.FromBase64String(passwordHash);
                byte[] salt = new byte[16];
                Array.Copy(passwordHashBytes, 0, salt, 0, 16);
                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);
                for (int i = 0; i < 20; ++i) if (passwordHashBytes[i + 16] != hash[i]) return false;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public User Login(LoginForm loginForm)
        {
            if (String.IsNullOrWhiteSpace(loginForm.Username) || String.IsNullOrWhiteSpace(loginForm.Password)) return null;

            User user = _context.Users.SingleOrDefault(user => user.Username == loginForm.Username);
            if (user == null) return null;

            if (!CorrectPassword(loginForm.Password, user.PasswordHash)) return null;

            return user;
        }

        private string CreatePasswordHash(string password)
        {
            try
            {
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);

                byte[] passwordHash = new byte[36];
                Array.Copy(salt, 0, passwordHash, 0, 16);
                Array.Copy(hash, 0, passwordHash, 16, 20);

                return Convert.ToBase64String(passwordHash);
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                return null;
            }
        }

        private bool IncorrectRoleName(string roleName)
        {
            if (roleName == RoleName.Admin) return false;
            else if (roleName == RoleName.Teacher) return false;
            else if (roleName == RoleName.Creator) return false;
            return true;
        }

        public User Register(RegisterForm registerForm)
        {
            if (String.IsNullOrWhiteSpace(registerForm.Password)) return null;
            if (IncorrectRoleName(registerForm.Role)) return null;

            if (_context.Users.Any(_user => _user.Username == registerForm.Username)) return null;

            string passwordHash = CreatePasswordHash(registerForm.Password);
            if (passwordHash == null) return null;

            User user = _mapper.Map<User>(registerForm);
            user.PasswordHash = passwordHash;

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public void DeleteUsers()
        {
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();
        }

        public bool DeleteUserById(int id)
        {
            User user = _context.Users.FirstOrDefault(user => user.Id == id);

            if (user == null) return false;

            _context.Users.Remove(user);
            _context.SaveChanges();

            return true;
        }
    }
}
