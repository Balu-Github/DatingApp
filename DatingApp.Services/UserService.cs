using AutoMapper;
using DatingApp.Contracts;
using DatingApp.Data;
using DatingApp.DTO;
using DatingApp.Util.Helpers;
using System;
using System.Threading.Tasks;

namespace DatingApp.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ILikeRepo _likeRepo;
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo
            , ILikeRepo likeRepo
            , IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _likeRepo = likeRepo;
        }

        public async Task<PagedList<User>> GetUsers(UserParams pageParams)
        {
            var users = await _userRepo.GetUsers(pageParams);
            return users;
        }

        public async Task<UserForDetailedDto> GetUser(int id)
        {
            var user = await _userRepo.GetUserById(id);

            var _user = _mapper.Map<UserForDetailedDto>(user);
            return _user;
        }

        public async Task<UserForUpdateDto> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            var user = await _userRepo.GetUserById(id);

            if (user == null)
                return null;

            _mapper.Map(userForUpdateDto, user);
            await _userRepo.Edit(user);
            return userForUpdateDto;
        }

        public async Task UpdateUserActivity(int userId)
        {
            var user = await _userRepo.GetUserById(userId);            
            user.LastActive = DateTime.Now;
            await _userRepo.Edit(user);            
        }

        public async Task<LikeDto> GetLike(int userId, int recipientId)
        {
            var like = await _userRepo.GetLike(userId, recipientId);           
            if (like == null)
                return null;
            var _like = _mapper.Map<LikeDto>(like);
            return _like;
        }

        public async Task<LikeDto> AddLike(LikeDto likeDto)
        {
            var _like = _mapper.Map<Like>(likeDto);
            await _likeRepo.Add(_like);        
            return likeDto;
        }
    }
}
