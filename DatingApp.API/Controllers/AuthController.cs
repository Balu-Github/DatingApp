using AutoMapper;
using DatingApp.Contracts;
using DatingApp.Data;
using DatingApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{   
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;

        public AuthController(
            IConfiguration config
            , IUserRepo userRepo
            , IMapper mapper
            , UserManager<User> userManager
            , SignInManager<User> signInManager
            , RoleManager<Role> roleManager)
        {
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

       
        // POST api/<AuthController>
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            //Bulk User Insert
            //await BulkUserInsert();

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var result = await _userManager.CreateAsync(userToCreate, userForRegisterDto.Password);

            var userToReturn = _mapper.Map<UserForDetailedDto>(userToCreate);

            if (result.Succeeded)
            {
                //return CreatedAtRoute("GetUser", new { controller = "Users", id = userToCreate.Id }, userToReturn);
                return Ok(userToReturn);
            }
            return BadRequest(result.Errors);
        }

      

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.Username);

            var result = await _signInManager.CheckPasswordSignInAsync(user, userForLoginDto.Password, false);

            if(result.Succeeded)
            {
                var appUser = _mapper.Map<UserForDetailedDto>(user);

                return Ok(new
                {
                    token = await GenerateJwtToken(user),
                    user = appUser
                });
            }
            return Unauthorized();           
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task BulkUserInsert()
        {
            var userData = System.IO.File.ReadAllText(@"E:\Projects\DatingApp\DatingApp.Data\bin\Debug\netcoreapp3.1\Data\UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userData);

            //Create some roles
            var roles = new List<Role>
            {
                new Role() {Name = "Member"},
                new Role() {Name = "Admin"},
                new Role() {Name = "Moderator"},
                new Role() {Name = "VIP"}
            };

            foreach (var role in roles)
            {
                _roleManager.CreateAsync(role).Wait();
            }

            foreach (var user in users)
            {
                _userManager.CreateAsync(user, "password").Wait();
                await _userManager.AddToRoleAsync(user, "Member");
            }

            var adminUser = new User
            {
                UserName = "Admin"
            };

            var result = _userManager.CreateAsync(adminUser, "password").Result;

            if(result.Succeeded)
            {
                var admin = _userManager.FindByNameAsync("Admin").Result;
                await _userManager.AddToRoleAsync(admin, "Admin");
                await _userManager.AddToRoleAsync(admin, "Moderator");
            }
        }
    }
}
