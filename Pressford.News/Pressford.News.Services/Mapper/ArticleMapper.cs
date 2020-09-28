using System;
using AutoMapper;
using Pressford.News.Model;
using entity = Pressford.News.Entities;

namespace Pressford.News.Services.Mapper
{
    public class ArticleMapper : Profile
    {
        public ArticleMapper()
        {
            CreateMap<Article, entity.Article>()
                    //.ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.DatePublished,
                                opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.DateModified,
                                opt => opt.MapFrom(src => DateTime.Now))
            .ReverseMap();
        }
    }
}