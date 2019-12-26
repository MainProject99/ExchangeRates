using AutoMapper;
using Model.Models;
using RESTfulAPIs.DTO;
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
        }
    }
}
