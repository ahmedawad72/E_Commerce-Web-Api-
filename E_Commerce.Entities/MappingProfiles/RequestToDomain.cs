using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.MappingProfiles
{
    public class RequestToDomain: Profile
    {
        public RequestToDomain()
        {

            #region ApplicationUser
            CreateMap<UserRegistrationRequestDto, ApplicationUser>()
                .ForMember(
                    dest => dest.UserName, option => option.MapFrom(src => src.Email)
                );

            CreateMap<UserLoginRequestDto, ApplicationUser>()
                .ForMember(
                    dest => dest.UserName, option => option.MapFrom(src => src.Email)
                );

            #endregion

            #region Item
            CreateMap<CreateItemRequestDto, Item>()
                .ForMember(
                        dest => dest.Image, opt => opt.MapFrom(src => ConvertImageFormIntoBytesArray(src.ImageFile))
                );
            
            CreateMap<UpdateItemRequestDto, Item>()
                .ForMember(
                        dest => dest.Image, opt => opt.MapFrom(src => ConvertImageFormIntoBytesArray(src.ImageFile))
                );

            #endregion

            #region CategotyItem

            //CreateMap<CreateItemRequestDto, Item>()
            //     .ForMember(
            //       dest => dest.Image, opt => opt.MapFrom(src => src.ImageFile)
            //     );
            #endregion

            #region Category

            CreateMap<CreateCategoryRequestDto, Category>();
             
            #endregion
         
            
            CreateMap<CreateOrderRequestDto, Order>().ReverseMap();
        }

        private  byte[] ConvertImageFormIntoBytesArray(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                 imageFile.CopyToAsync(memoryStream);
                  return memoryStream.ToArray();
            }
        }
    }
}
