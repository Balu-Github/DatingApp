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

        Task<PagedList<User>> GetUsers(PageParams pageParams);
        Task<User> GetUserById(int userId);
    }
}
