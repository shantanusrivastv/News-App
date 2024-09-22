using System;
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
		private readonly string _userName = string.Empty;

		public ArticleServices(IRepository<entity.Article> repository,
							   IMapper mapper, 
							   IHttpContextAccessor httpContextAccessor)
		{
			_repository = repository;
			_mapper = mapper;
			_userName = httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}

		public async Task<ReadArticle> CreateArticle(ArticleBase article)
		{
			var authorEntity = _mapper.Map<entity.Article>(article);
			authorEntity.Author = _userName ?? throw new ApplicationException("User is not logged in");
			var result = await _repository.AddAsync(authorEntity);
			return _mapper.Map<ReadArticle>(result);
		}

		public Task<ReadArticle> GetSingleArticle(int articleId)
		{
			Expression<Func<entity.Article, bool>> predicate = (x) => x.Id == articleId;
			var result = _repository.FindBy(predicate).SingleOrDefault();
			return Task.FromResult(_mapper.Map<ReadArticle>(result));
		}

		public Task<IList<ReadArticle>> GetAllArticles()
		{
			var entityArticles = _repository.GetAll().ToList();
			return Task.FromResult(_mapper.Map<IList<ReadArticle>>(entityArticles));
		}

		public Task<bool> RemoveArticle(int articleId)
		{
			if (!IsArticleOwner(_userName, articleId))
			{
				return Task.FromResult(false);
			}
			return _repository.Delete(articleId);
		}

		public async Task<ReadArticle> UpdateArticle(UpdateArticle article)
		{
			if (_userName == null || !IsArticleOwner(_userName, article.Id))
			{
				return null;
			}
			var entityArticle = _mapper.Map<entity.Article>(article);
			entityArticle.Author = _userName;
			var dbreturn = await _repository.UpdateAsync(entityArticle);
			return _mapper.Map<ReadArticle>(dbreturn);
		}

		private bool IsArticleOwner(string userName, int articleId)
		{
			return _repository.FindBy(x => x.Author == userName && x.Id == articleId).Any();
		}
	}
}