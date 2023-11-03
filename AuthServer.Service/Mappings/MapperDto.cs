using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AutoMapper;

namespace AuthServer.Service.Mappings
{
    public class MapperDto : Profile
    {
        public MapperDto()
        {
            this.CreateMap<AppUserDto, AppUser>().ReverseMap();
            this.CreateMap<ProductDto, Product>().ReverseMap();
        }
    }
}
