using AutoMapper;
using entity = Pressford.News.Entities;
using model = Pressford.News.Model;

namespace Pressford.News.Services.Mapper
{
	public class PressfordMapper : Profile
	{
		public PressfordMapper()
		{
			//we probbably need to add some ignore like.ForMember(dest => dest.Id, opt => opt.Ignore());
			CreateMap<model.ReadArticle, entity.Article>()
				.ForMember(dest => dest.Id,
					  opt => opt.MapFrom(readArticle => readArticle.ArticleId))
			.ReverseMap();

			CreateMap<model.CreateArticle, entity.Article>()
			.ReverseMap();

			CreateMap<model.UpdateArticle, entity.Article>()
			 .ForMember(dest => dest.DateModified,
						opt => opt.Ignore())
			 .ReverseMap();

			CreateMap<model.PatchArticle, entity.Article>()
			 .ForMember(dest => dest.DateModified,
						opt => opt.Ignore())

		     .ForMember(dest => dest.Id,
						opt => opt.MapFrom(updatedArticle => updatedArticle.ArticleId))

			 .ForMember(dest => dest.DatePublished,
						opt => opt.Ignore())
						.ReverseMap();
			
			CreateMap<model.ReadArticle, model.UpdateArticle>().ReverseMap();

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