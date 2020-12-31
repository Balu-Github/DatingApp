using AutoMapper;
using DatingApp.Contracts;
using DatingApp.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo userRepo
            , IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserForListDto>> GetUsers()
        {
            var users = await _userRepo.GetAll();

            var _users = _mapper.Map<IEnumerable<UserForListDto>>(users);

            return _users;
        }

        public async Task<UserForDetailedDto> GetUser(int id)
        {
            var user = await _userRepo.GetById(id);

            var _user = _mapper.Map<UserForDetailedDto>(user);
            return _user;
        }

        public async Task<UserForUpdateDto> UpdateUser(int id, UserForUpdateDto userForUpdateDto)
        {
            var user = await _userRepo.GetById(id);

            if (user == null)
                return null;

            _mapper.Map(userForUpdateDto, user);
            await _userRepo.Edit(user);
            return userForUpdateDto;
        }

        public async Task UpdateUserActivity(int userId)
        {
            var user = await _userRepo.GetById(userId);            
            user.LastActive = DateTime.Now;
            await _userRepo.Edit(user);            
        }
    }
}
