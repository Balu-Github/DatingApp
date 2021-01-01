using DatingApp.Contracts;
using DatingApp.Contracts.Generic;
using DatingApp.Data;
using DatingApp.Repo.Generic;

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
