using DatingApp.Contracts.Generic;
using DatingApp.Data;
using System;

namespace DatingApp.Contracts
{
    public interface IUserRepo : IGenericRepo<User>
    {
       void SeedUsers();
    }
}
