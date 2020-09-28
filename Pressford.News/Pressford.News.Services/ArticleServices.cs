using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Pressford.News.Data;
using Pressford.News.Model;
using entity = Pressford.News.Entities;

namespace Pressford.News.Services
{
    public class ArticleServices : IArticleServices
    {
        private IRepository<entity.Article> _repository;
        private readonly IMapper _mapper;

        public ArticleServices(IRepository<entity.Article> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Article> CreateArticle(Article article)
        {
            var result = await _repository.AddAsync(_mapper.Map<entity.Article>(article));
            return _mapper.Map<Article>(result);
        }

        public async Task<IList<Article>> GetAllArticles()
        {
            var entityArticles = _repository.GetAll().ToList();
            return _mapper.Map<IList<Article>>(entityArticles);
        }
    }
}