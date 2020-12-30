using DatingApp.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserForListDto>> GetUsers();

        Task<UserForDetailedDto> GetUser(int id);

        Task<UserForUpdateDto> UpdateUser(int id, UserForUpdateDto userForUpdateDto);
    }
}
