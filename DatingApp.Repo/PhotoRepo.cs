using DatingApp.Contracts;
using DatingApp.Contracts.Generic;
using DatingApp.Data;
using DatingApp.Data.Data;
using DatingApp.Repo.Generic;
using System;

namespace DatingApp.Repo
{
    public class PhotoRepo : GenericRepo<Photo>, IPhotoRepo
    {
        private readonly IUnitOfWork _unitOfWork;
        public PhotoRepo(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public void SeedUsers()
        //{
        //    Seed.SeedUsers(_unitOfWork.Context);
        //}
    }
}
