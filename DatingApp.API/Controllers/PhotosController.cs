using DatingApp.Contracts;
using DatingApp.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photo = await _photoService.GetPhoto(id);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotForUser(int userId, [FromForm]PhotoCreationDto photoCreationDto)
        {
            var photoFromRepo = await _photoService.AddPhoto(userId, photoCreationDto);
            return Ok(photoFromRepo);
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            //TODO: Need to add all contorller methods
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (id < 0)
                return BadRequest();

            if (await _photoService.IsMainPhoto(id))
                return BadRequest("This is already the main photo");

            if(await _photoService.UpdateExistingMainPhoto(userId))
            {
                var photo = await _photoService.SetMainPhoto(id);
                return Ok(photo);
            }
            return BadRequest("could not set photo to main");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            if (id < 0)
                return BadRequest();

            if (await _photoService.IsMainPhoto(id))
                return BadRequest("You cannot delete your main photo");

            if (await _photoService.DeletePhoto(userId, id))
                return Ok();
            else
                return BadRequest("Failed to delete a Photo");
        }
    }
}
