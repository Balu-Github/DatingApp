using DatingApp.Contracts;
using DatingApp.Contracts.Generic;
using DatingApp.Data;
using DatingApp.Data.Data;
using DatingApp.Repo.Generic;
using DatingApp.Util.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<PagedList<User>> GetUsers(UserParams pageParams)
        {
            var users = _unitOfWork.Context.Users.Include(p => p.Photos)
                            .OrderByDescending(u => u.LastActive).AsQueryable();

            users = users.Where(u => u.Id != pageParams.UserId);

            users = users.Where(u => u.Gender == pageParams.Gender);

            if(pageParams.Likers)
            {
                var userLikers = await GetUserLikes(pageParams.UserId, pageParams.Likers);
                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if (pageParams.Likees)
            {
                var userLikees = await GetUserLikes(pageParams.UserId, pageParams.Likers);
                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (pageParams.MinAge != 18 || pageParams.MaxAge != 99)
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

        public async Task<Like> GetLike(int userId,  int recipientId)
        {
            return await _unitOfWork.Context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipientId);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers)
        {
            var user = await _unitOfWork.Context.Users
                                .Include(x => x.Likers)
                                .Include(x => x.Likees)
                                .FirstOrDefaultAsync(u => u.Id == id);
            if(likers)
            {
                return user.Likers.Where(u => u.LikeeId == id).Select(u => u.LikerId);
            }
            else
            {
                return user.Likees.Where(u => u.LikerId == id).Select(u => u.LikeeId);
            }
        }
    }
}
