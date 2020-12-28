using DatingApp.Contracts;
using DatingApp.Contracts.Generic;
using DatingApp.Data;
using DatingApp.Data.Data;
using DatingApp.Repo.Generic;
using System;

namespace DatingApp.Repo
{
    public class UserRepo : GenericRepo<User>, IUserRepo
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserRepo(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void SeedUsers()
        {
            Seed.SeedUsers(_unitOfWork.Context);
        }
    }
}
