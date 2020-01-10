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
            CreateMap<CurrencyRequestDto, CurrencyResponceDto>().ReverseMap();

            CreateMap<Currencies, CurrencyResponceDto>().ReverseMap();
            //.ForMember(c=>c.to, d => d.MapFrom(src=>src.Name));
            CreateMap<CurrencyFrom, CurrencyResponceDto>().ReverseMap();
                //.ForMember(c => c.from, d => d.MapFrom(src => src.Name));
        }
    }
}
