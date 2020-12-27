using System;

namespace DatingApp.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string PasswordSalt { get; set; }
    }
}
