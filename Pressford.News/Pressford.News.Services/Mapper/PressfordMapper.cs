using AutoMapper;
using entity = Pressford.News.Entities;
using model = Pressford.News.Model;

namespace Pressford.News.Services.Mapper
{
    public class PressfordMapper : Profile
    {
        public PressfordMapper()
        {
            CreateMap<model.Article, entity.Article>()
            .ReverseMap();

            CreateMap<entity.UserLogin, model.UserInfo>()
            .ForMember(dest => dest.Name,
                      opt => opt.MapFrom(userLogin => $"{userLogin.User.FirstName} {userLogin.User.LastName}"))

            .ForMember(dest => dest.Username,
                      opt => opt.MapFrom(userLogin => userLogin.Username))

            .ForMember(dest => dest.Role,
                      opt => opt.MapFrom(userLogin => userLogin.Role))

            .ForMember(dest => dest.Token,
                      opt => opt.MapFrom(userLogin => userLogin.Token));
        }
    }
}