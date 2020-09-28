using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public Task<Article> GetSingleArticle(int articleId)
        {
            Expression<Func<entity.Article, bool>> predicate = (x) => x.Id == articleId;
            var result = _repository.FindBy(predicate).SingleOrDefault();
            return Task.FromResult(_mapper.Map<Article>(result));
        }

        public Task<IList<Article>> GetAllArticles()
        {
            var entityArticles = _repository.GetAll().ToList();
            return Task.FromResult(_mapper.Map<IList<Article>>(entityArticles));
        }

        public Task<bool> RemoveArticle(int articleId)
        {
            return _repository.Delete(articleId);
        }

        public Task<Article> UpdateArticle(Article article)
        {
            var entityArticle = _mapper.Map<entity.Article>(article);
            return Task.FromResult(_mapper.Map<Article>(_repository.UpdateAsync(entityArticle).Result));
        }
    }
}