using AutoMapper;
using BookStore.API.DTOs;
using BookStore.API.Models;

namespace BookStore.API.Helpers.AutoMapperConfigurations
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegisterDto, ApplicationUser>();
        }
    }
}
