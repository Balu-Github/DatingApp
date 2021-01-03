using DatingApp.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Contracts
{
    public interface IPhotoService
    {
        Task<PhotoForDetailedDto> AddPhoto(int userId, PhotoCreationDto photoCreationDto);
        Task<PhotoForDetailedDto> GetPhoto(int id);
        Task<PhotoForDetailedDto> SetMainPhoto(int id);
        Task<bool> UpdateExistingMainPhoto(int userId);
        Task<bool> IsMainPhoto(int id);
        Task<bool> DeletePhoto(int id);
    }
}
