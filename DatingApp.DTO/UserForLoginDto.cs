using System;
using System.Collections.Generic;
using System.Text;

namespace DatingApp.DTO
{
    public class UserForLoginDto
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
