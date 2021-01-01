using DatingApp.Contracts.Generic;
using DatingApp.Data;
using DatingApp.Util.Helpers;
using System;
using System.Threading.Tasks;

namespace DatingApp.Contracts
{
    public interface IUserRepo : IGenericRepo<User>
    {
       void SeedUsers();
        Task<PagedList<User>> GetUsers(UserParams pageParams);
        Task<User> GetUserById(int userId);
        Task<Like> GetLike(int userId, int recipientId);
    }
}
