using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Pressford.News.Data;
using Pressford.News.Entities;
using Pressford.News.Model;
using Pressford.News.Services.Interfaces;
using entity = Pressford.News.Entities;
using JsonPatchArticle = Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Pressford.News.Model.UpdateArticle>;

namespace Pressford.News.Services
{
	public class ArticleServices : IArticleServices
	{
		private readonly IRepository<entity.Article> _repository;
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly string _userName = string.Empty;

		public ArticleServices(IRepository<entity.Article> repository,
							   IUserRepository userRepository,
							   IMapper mapper,
							   IHttpContextAccessor httpContextAccessor)
		{
			_repository = repository;
			_userRepository = userRepository;
			_mapper = mapper;
			_userName = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}

		public async Task<ReadArticle> CreateArticle(ArticleBase article)
		{
			var authorEntity = _mapper.Map<entity.Article>(article);
			authorEntity.Author = _userName ?? throw new ApplicationException("User is not logged in");
			var result = await _repository.AddAsync(authorEntity);
			return _mapper.Map<ReadArticle>(result);
		}

		public async Task<ReadArticle> GetSingleArticle(int articleId)
		{
			Expression<Func<entity.Article, bool>> predicate = (x) => x.Id == articleId;
			var result = await _repository.FindBy(predicate).SingleOrDefaultAsync();
			return (_mapper.Map<ReadArticle>(result));
		}

		public async Task<IList<ReadArticle>> GetAllArticles()
		{
			// GetAuthorWithinRange();
			var entityArticles = await _repository.GetAll().ToListAsync();
			return _mapper.Map<IList<ReadArticle>>(entityArticles);
		}	
		
		public async Task<IList<User>> GetAuthorWithinRange()
		{
			var res = await _userRepository.GetPublishedArticleWithinRange(DateTime.Now.AddYears(-5), DateTime.Now);
			return res;
		}

		public async Task<bool> RemoveArticle(int articleId)
		{
			if (!await IsArticleOwner(_userName, articleId))
			{
				return false;
			}
			return await _repository.Delete(articleId);
		}

		public async Task<ReadArticle> UpdateArticle(UpdateArticle article)
		{
			if (_userName == null || !await IsArticleOwner(_userName, article.Id))
			{
				return null;
			}
			var entityArticle = _mapper.Map<entity.Article>(article);
			entityArticle.Author = _userName;
			var updatedArticle = await _repository.UpdateAsync(entityArticle);
			return _mapper.Map<ReadArticle>(updatedArticle);
		}

		public async Task<(ReadArticle, IEnumerable<ValidationResult>)> PatchArticle(int articleId, JsonPatchArticle patchArticle)
		{
			var validationResults = new List<ValidationResult>();
			if (_userName == null || !await IsArticleOwner(_userName, articleId))
			{
				return (null, validationResults);
			}

			Expression<Func<entity.Article, bool>> predicate = (x) => x.Id == articleId;
			var existingArticle = _repository.FindBy(predicate).SingleOrDefault();

			if (existingArticle == null)
				return (null, validationResults);

			var articleToUpdate = _mapper.Map<UpdateArticle>(existingArticle);

			var (validationResult, patchedArticle) = ValidateAndPatchDocument(patchArticle, articleToUpdate, validationResults);

			if (validationResult.Any())
			{
				return (null, validationResult);
			}

			_mapper.Map(patchedArticle, existingArticle);
			var updatedArticle = await _repository.UpdateAsync(existingArticle);
			return (_mapper.Map<ReadArticle>(updatedArticle), validationResult);
		}

		private async Task<bool> IsArticleOwner(string userName, int articleId)
		{
			return await _repository.FindBy(x => x.Author == userName && x.Id == articleId).AnyAsync();
		}

		//Todo: We can make it to a generic async method
		private (IEnumerable<ValidationResult>, UpdateArticle) ValidateAndPatchDocument(JsonPatchArticle patchArticle,
																						UpdateArticle articleToUpdate,
																						List<ValidationResult> validationResults)
		{
			try
			{
				patchArticle.ApplyTo(articleToUpdate, error =>
				{
					// Capture patch errors and add them to validation results
					validationResults.Add(new ValidationResult(error.ErrorMessage));
				});
				var validationContext = new ValidationContext(articleToUpdate, serviceProvider: null, items: null);

				Validator.TryValidateObject(articleToUpdate, validationContext, validationResults, validateAllProperties: true);
				return (validationResults, articleToUpdate);
			}
			catch (Exception ex)
			{
				validationResults.Add(new ValidationResult($"Patch application failed: {ex.Message}"));
				return (validationResults, articleToUpdate);
			}
		}
	}
}