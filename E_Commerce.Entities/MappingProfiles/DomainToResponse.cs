﻿using AutoMapper;
using E_Commerce.Entities.DbSet;
using E_Commerce.Entities.DTOs.Requests;
using E_Commerce.Entities.DTOs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Entities.MappingProfiles
{
    public class DomainToResponse: Profile
    {
        public DomainToResponse()
        {
            #region Item
            CreateMap<Item, CreateItemResponseDto>();
            CreateMap<Item, GetItemsResponseDto>()
                  .ForMember(dest => dest.FinalPrice, opt => opt.MapFrom(src => src.FinalPrice))
                  .ForMember(dest => dest.CategoriesIds, opt => opt.MapFrom(src => src.Categories.Select(c => c.Id))
                ).ReverseMap();
         
            #endregion

            #region Category
            CreateMap<Category, GetCategoryResponseDto>().ReverseMap();
            CreateMap<Category, CreateCategoryResponseDto>().ReverseMap();
            #endregion

            CreateMap<CartItem, GetItemsResponseDto>().ReverseMap();
         
            
            
            CreateMap<Order, GetOrderResponseDto>().ReverseMap();
        }



    }
}
