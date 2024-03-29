﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Pressford.News.Data;
using Pressford.News.Model;
using Pressford.News.Services.Interfaces;
using entity = Pressford.News.Entities;

namespace Pressford.News.Services
{
    public class ArticleServices : IArticleServices
    {
        private readonly IRepository<entity.Article> _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArticleServices(IRepository<entity.Article> repository,
                               IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Article> CreateArticle(Article article)
        {
            var userName = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var authorEntity = _mapper.Map<entity.Article>(article);
            authorEntity.Author = userName ?? throw new ApplicationException("User is not logged in");

            var result = await _repository.AddAsync(authorEntity);
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
            var userName = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!IsArticleOwner(userName, articleId))
            {
                return Task.FromResult(false);
            }
            return _repository.Delete(articleId);
        }

        public async Task<Article> UpdateArticle(Article article)
        {
            var userName = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userName == null || !IsArticleOwner(userName, article.Id))
            {
                return null;
            }
            var entityArticle = _mapper.Map<entity.Article>(article);
            entityArticle.Author = userName;
            var dbreturn = await _repository.UpdateAsync(entityArticle);
            return _mapper.Map<Article>(dbreturn);
        }

        private bool IsArticleOwner(string userName, int articleId)
        {
            return _repository.FindBy(x => x.Author == userName && x.Id == articleId).Any();
        }
    }
}