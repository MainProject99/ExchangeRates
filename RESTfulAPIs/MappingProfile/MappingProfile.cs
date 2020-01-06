using AutoMapper;
using Model.Models;
using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace RESTfulAPIs.MappingProfile
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<UpdateDto, User>();
            CreateMap<CurrencyResponceDto, CurrencyRequestDto>().ReverseMap();
        }
    }
}
