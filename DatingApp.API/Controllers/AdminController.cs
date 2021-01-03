using CloudinaryDotNet.Actions;
using DatingApp.Contracts;
using DatingApp.Data;
using DatingApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly DatingAppContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IPhotoService _photoService;

        public AdminController(DatingAppContext context, 
            UserManager<User> userManager,
            IPhotoService photoService)
        {
            _context = context;
            _userManager = userManager;
            _photoService = photoService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("usersWithRoles")]
        public async Task<IActionResult> GetUserWithRoles()
        {
            var userList = await _context.Users.OrderBy(u => u.UserName).Select(user => new
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = (from userRole in user.UserRoles
                         join role in _context.Roles on userRole.RoleId equals role.Id
                         select role.Name).ToList()
            }).ToListAsync();

            return Ok(userList);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("editRoles/{userName}")]
        public async Task<IActionResult> EditRoles(string userName,  RoleEditDto roleEditDto)
        {
            var user = await _userManager.FindByNameAsync(userName);

            var userRoles = await _userManager.GetRolesAsync(user);

            var seelctedRoles = roleEditDto.RoleNames;

            seelctedRoles = seelctedRoles ?? new string[] { };

            var result = await _userManager.AddToRolesAsync(user, seelctedRoles.Except(userRoles));

            if (!result.Succeeded)
                return BadRequest("Failed to add to roles");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(seelctedRoles));

            if(!result.Succeeded)
                return BadRequest("Failed to remove to roles");

            return Ok(await _userManager.GetRolesAsync(user));

        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("photosForModeration")]
        public async  Task<IActionResult> GetPhotosForModeration()
        {
            var photos = await _context.Photos.Where(p => p.IsApproved == false)
                 .Select(u => new
                 {
                     Id = u.Id,
                     UserName = u.User.UserName,
                     Url = u.Url,
                     IsApproved = u.IsApproved
                 }).ToListAsync();

            return Ok(photos);                
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("approvePhoto/{photoId}")]
        public async Task<IActionResult> ApprovePhoto(int photoId)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == photoId);
            photo.IsApproved = true;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize(Policy = "ModeratePhotoRole")]
        [HttpGet("rejectPhoto/{photoId}")]
        public async Task<IActionResult> RejectPhoto(int photoId)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == photoId);

            if (photo.IsMain)
                return BadRequest("You cannot reject the main photo");

            if (await _photoService.DeletePhoto(photo.Id))
                return Ok();
            return BadRequest("Unable to reject the photo");
        }
    }
}
