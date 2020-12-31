using DatingApp.Contracts;
using DatingApp.Contracts.Generic;
using DatingApp.Data;
using DatingApp.Data.Data;
using DatingApp.Repo.Generic;
using DatingApp.Util.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<PagedList<User>> GetUsers(PageParams pageParams)
        {
            var users = _unitOfWork.Context.Users.Include(p => p.Photos)
                            .OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != pageParams.UserId);

            users = users.Where(u => u.Gender == pageParams.Gender);

            if(pageParams.MinAge != 18 || pageParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-pageParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-pageParams.MinAge - 1);

                users = users.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if(!string.IsNullOrEmpty(pageParams.OrderBy))
            {
                switch (pageParams.OrderBy)
                {
                    case "created":
                        users = users.OrderByDescending(u => u.Created);
                        break;

                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, pageParams.PageNumber, pageParams.PageSize);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _unitOfWork.Context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == userId);
        }
    }
}
