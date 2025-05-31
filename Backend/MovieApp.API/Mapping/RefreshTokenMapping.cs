using AutoMapper;
using MovieApp.DTO.DTOs.RefreshTokenDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class RefreshTokenMapping : Profile
    {
        public RefreshTokenMapping()
        {
            CreateMap<RefreshToken, RefreshTokenListDto>();
            CreateMap<RefreshToken, RefreshTokenDetailDto>();
        }
    }
}
