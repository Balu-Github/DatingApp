using AutoMapper;
using DatingApp.Contracts;
using DatingApp.Data;
using DatingApp.DTO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public AuthService(
            IUserRepo userRepo
            , IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }        

        public async Task<UserForLoginDto> Login(string username, string password)
        {
            var user = (await _userRepo.Find(u => u.Username == username)).FirstOrDefault();

            if (user == null)
                return null;
            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;
            var userForLogin = _mapper.Map<UserForLoginDto>(user);
            return userForLogin;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
                return true;
            }
        }

        public async Task<UserForDetailedDto> Register(UserForRegisterDto user, string password)
        {
            //Seed users Data
            //_userRepo.SeedUsers();

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var _user = _mapper.Map<User>(user);
            _user.PasswordHash = passwordHash;
            _user.PasswordSalt = passwordSalt;          
            var savedUser = await _userRepo.Add(_user);
            return _mapper.Map<UserForDetailedDto>(savedUser);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<bool> UserExists(string username)
        {
            if ((await _userRepo.Find(u => u.Username == username)).Any())
                return true;
            return false;
        }
    }
}
