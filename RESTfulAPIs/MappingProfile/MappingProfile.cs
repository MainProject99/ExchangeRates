using AutoMapper;
using Model.Models;
using BusinessLogicLayer.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using CurrencyAPI.ApiDTO;

namespace RESTfulAPIs.MappingProfile
{
    class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterDto, User>();
            CreateMap<UpdateDto, User>();
            

            CreateMap<Currency, CurrencyDefaultInfoDTO>()
                .ForMember(c => c.UserID, src => src.MapFrom(c => c.UserId))
                .ForMember(c => c.CurrencyFromDefault, src => src.MapFrom(c => c.CurrencyFrom))
                .ForMember(c => c.CurrencyToDefault, src => src.MapFrom(c => c.CurrencyTo));

            CreateMap<CurrencyRequestDto, Currency>()
                .ForMember(c => c.CurrencyTo, d => d.MapFrom(src => src.to))
                .ForMember(c => c.CurrencyFrom, d => d.MapFrom(src => src.from));
            


       

        }
    }
}
