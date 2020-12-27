using DatingApp.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Contracts
{
    public interface IAuthService
    {
        Task<ICollection<UserDTO>> GetUsers();

        Task<UserDTO> Register(UserDTO user, string password);

        Task<UserDTO> Login(string username, string password);

        Task<bool> UserExists(string username);
    }

}
