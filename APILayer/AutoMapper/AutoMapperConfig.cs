using System;
using AutoMapper;
using DTOLayer.DTOs.User;
using EntityLayer.Entites;

namespace APILayer.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<UsersGetDTO, User>().ReverseMap();
        }
    }

}

