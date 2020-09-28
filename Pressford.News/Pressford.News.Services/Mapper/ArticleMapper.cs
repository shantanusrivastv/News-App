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

            .ReverseMap();
        }
    }
}