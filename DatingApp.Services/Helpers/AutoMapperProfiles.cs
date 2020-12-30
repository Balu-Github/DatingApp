using AutoMapper;
using DatingApp.Data;
using DatingApp.DTO;
using DatingApp.Util.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatingApp.Services.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<User, UserForDetailedDto>()
                 .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                 .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoForDetailedDto>();
            CreateMap<UserForUpdateDto, User>();
        }
    }
}
