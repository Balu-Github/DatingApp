using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.Contracts;
using DatingApp.Data;
using DatingApp.DTO;
using DatingApp.Util.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRepo _photoRepo;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _clodinaryConfig;
        private Cloudinary _clodinary;

        public PhotoService(IPhotoRepo photoRepo
            , IUserRepo userRepo
            , IMapper mapper
            , IOptions<CloudinarySettings> clodinaryConfig)
        {
            _photoRepo = photoRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _clodinaryConfig = clodinaryConfig;

            Account acc = new Account()
            {
                Cloud = _clodinaryConfig.Value.CloudName,
                ApiKey = _clodinaryConfig.Value.ApiKey,
                ApiSecret = _clodinaryConfig.Value.ApiSecret
            };
            _clodinary = new Cloudinary(acc);
        }


        public async Task<PhotoForDetailedDto> AddPhoto(int userId, PhotoCreationDto photoCreationDto)
        {
            var user = await _userRepo.GetUserById(userId);

            var file = photoCreationDto.File;
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                                    .Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    uploadResult = _clodinary.Upload(uploadParams);
                }
            }

            photoCreationDto.Url = uploadResult.SecureUrl.ToString();
            photoCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoCreationDto);
            if (!user.Photos.Any(p => p.IsMain))
                photo.IsMain = true;

            photo.UserId = userId;
            var savedPhoto = await _photoRepo.Add(photo);
            var photoForDetailedDto = _mapper.Map<PhotoForDetailedDto>(savedPhoto);
            return photoForDetailedDto;
        }

        public async Task<PhotoForDetailedDto> GetPhoto(int id)
        {
            var photo = await _photoRepo.GetById(id);
            var _photoDto = _mapper.Map<PhotoForDetailedDto>(photo);
            return _photoDto;
        }

        public async Task<PhotoForDetailedDto> SetMainPhoto(int id)
        {
            var photo = await _photoRepo.GetById(id);
            photo.IsMain = true;
            await _photoRepo.Edit(photo);
            var _photoDto = _mapper.Map<PhotoForDetailedDto>(photo);
            return _photoDto;
        }

        public async Task<bool> UpdateExistingMainPhoto(int userId)
        {
            var photo = (await _photoRepo.GetAll()).Where(u => u.UserId == userId).FirstOrDefault(p => p.IsMain);
            photo.IsMain = false;
            await _photoRepo.Edit(photo);
            return true;
        }

        public async Task<bool> IsMainPhoto(int id)
        {
            var photo = await _photoRepo.GetById(id);
            return photo.IsMain;
        }

        public async Task<bool> DeletePhoto(int id)
        {
            var photo = await _photoRepo.GetById(id);

            if(photo.PublicId != null)
            {
                var deletParams = new DeletionParams(photo.PublicId);
                var result = _clodinary.Destroy(deletParams);
                if (result.Result == "ok")
                {
                    await _photoRepo.Delete(photo);
                    return true;
                }
            }
            else
            {
                await _photoRepo.Delete(photo);
                return true;
            }            
            return false;
        }
    }
}
