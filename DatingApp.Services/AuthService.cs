using DatingApp.Contracts;
using DatingApp.Data;
using DatingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;

        public AuthService(
            IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ICollection<UserDTO>> GetUsers()
        {
            var allUsers = (await _userRepo.GetAll()).Select(u => new UserDTO()
            {
                Id = u.Id,
                Username = u.Username
            }).ToList();
            return allUsers;
        }

        public async Task<UserDTO> Login(string username, string password)
        {
            var user = (await _userRepo.Find(u => u.Username == username)).FirstOrDefault();

            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            return new UserDTO() { Id = user.Id, Username = user.Username };
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        public async Task<UserDTO> Register(UserDTO user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var _user = new User()
            {
                Username = user.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };           
            var savedUser = await _userRepo.Add(_user);
            user.Id = savedUser.Id;          
            return user;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if ((await _userRepo.Find(u => u.Username == username)).Any())
                return true;
            return false;
        }
    }
}
