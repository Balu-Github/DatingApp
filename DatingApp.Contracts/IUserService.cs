using DatingApp.Data;
using DatingApp.DTO;
using DatingApp.Util.Helpers;
using System.Threading.Tasks;

namespace DatingApp.Contracts
{
    public interface IUserService
    {
        Task<PagedList<User>> GetUsers(PageParams pageParams);
        Task<UserForDetailedDto> GetUser(int id);
        Task<UserForUpdateDto> UpdateUser(int id, UserForUpdateDto userForUpdateDto);
        Task UpdateUserActivity(int userId);
    }
}
