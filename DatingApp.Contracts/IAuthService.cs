using DatingApp.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Contracts
{
    public interface IAuthService
    {        
        Task<UserForDetailedDto> Register(UserForRegisterDto user, string password);

        Task<UserForLoginDto> Login(string username, string password);

        Task<bool> UserExists(string username);
    }

}
