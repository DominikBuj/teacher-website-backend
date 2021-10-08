using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TeacherWebsiteBackEnd.Entities;
using TeacherWebsiteBackEnd.Models;
using TeacherWebsiteBackEnd.DTOs;

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

        public async Task<bool> UserExistsByUsername(string username)
        {
            return await _context.Users.AnyAsync(user => user.Username == username);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
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
                Console.WriteLine("Incorrect password");
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }

        public async Task<User> Login(LoginForm loginForm)
        {
            try
            {
                User user = await _context.Users.SingleOrDefaultAsync(user => user.Username == loginForm.Username);
                if (user == null) return null;

                if (!CorrectPassword(loginForm.Password, user.PasswordHash)) return null;

                return user;
            }
            catch (Exception e)
            {
                Console.WriteLine("More than one user with the same username found");
                Console.WriteLine(e.Message);
                return null;
            }
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
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public async Task<User> Register(RegisterForm registerForm)
        {
            if (!Enum.TryParse(registerForm.Role, out UserRole userRole)) return null;

            bool userExists = await UserExistsByUsername(registerForm.Username);
            if (userExists) return null;

            string passwordHash = CreatePasswordHash(registerForm.Password);
            if (passwordHash == null) return null;

            User user = _mapper.Map<User>(registerForm);
            user.PasswordHash = passwordHash;

            EntityEntry<User> _user = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return _user.Entity;
        }

        public async void DeleteUsers()
        {
            _context.Users.RemoveRange(_context.Users);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserById(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(user => user.Id == id);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
