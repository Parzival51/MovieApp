using AutoMapper;
using MovieApp.DTO.DTOs.CommentDtos;
using MovieApp.Entity.Entities;

namespace MovieApp.API.Mapping
{
    public class CommentMapping : Profile
    {
        public CommentMapping()
        {
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<UpdateCommentDto, Comment>();

            CreateMap<Comment, CommentListDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsApproved, opt => opt.MapFrom(src => src.IsApproved));

            CreateMap<Comment, CommentDetailDto>()
                .IncludeBase<Comment, CommentListDto>();
        }
    }
}
