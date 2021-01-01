using DatingApp.Contracts;
using DatingApp.Contracts.Generic;
using DatingApp.Data;
using DatingApp.Data.Data;
using DatingApp.Repo.Generic;
using System;

namespace DatingApp.Repo
{
    public class LikeRepo : GenericRepo<Like>, ILikeRepo
    {
        private readonly IUnitOfWork _unitOfWork;
        public LikeRepo(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }
}
