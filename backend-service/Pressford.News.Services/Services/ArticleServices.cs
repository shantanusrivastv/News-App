﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Pressford.News.Data;
using Pressford.News.Entities;
using Pressford.News.Model;
using Pressford.News.Model.Helpers;
using Pressford.News.Model.ResourceParameters;
using Pressford.News.Services.Extensions;
using Pressford.News.Services.Interfaces;
using Pressford.News.Services.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using entity = Pressford.News.Entities;
using JsonPatchArticle = Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Pressford.News.Model.PatchArticle>;

namespace Pressford.News.Services.Services
{
    public class ArticleServices : IArticleServices
    {
        private readonly IRepository<Article> _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly string _userName = string.Empty;
        private readonly IPropertyMappingService _propertyMappingService;


        public ArticleServices(IRepository<Article> repository,
                               IUserRepository userRepository,
                               IMapper mapper,
                               IHttpContextAccessor httpContextAccessor,
                               IPropertyMappingService propertyMappingService
                               )
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
            _userName = httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            _propertyMappingService = propertyMappingService;
        }

        public async Task<ReadArticle> CreateArticle(CreateArticle article)
        {
            var articleEntity = _mapper.Map<Article>(article);
            articleEntity.Author = _userName ?? throw new ApplicationException("User is not logged in");
            var result = await _repository.AddAsync(articleEntity);
            return _mapper.Map<ReadArticle>(result);
        }

        public async Task<IEnumerable<ReadArticle>> CreateArticlCollection(IEnumerable<CreateArticle> articlecollection)
        {
            var articleEntity = _mapper.Map<IEnumerable<Article>>(articlecollection);
            if (_userName == null)
                throw new ApplicationException("User is not logged in");

            Parallel.ForEach(articleEntity, article => article.Author = _userName);

            var articleEntitities = await _repository.AddListAsync(articleEntity);
            return _mapper.Map<IEnumerable<ReadArticle>>(articleEntitities);
        }

        public async Task<ReadArticle> GetSingleArticle(int articleId)
        {
            Expression<Func<Article, bool>> predicate = (x) => x.Id == articleId;
            var result = await _repository.FindBy(predicate).SingleOrDefaultAsync();
            return _mapper.Map<ReadArticle>(result);
        }

        public async Task<PagedList<ReadArticle>> GetAllArticles(ArticleResourceParameters articleResource)
        {
            // GetAuthorWithinRange();
            //var res = await GetAuthorWithTitle();

            //There is no issue in running this first as I am using IQueryable
            //this greatly simplify the optional serarcha nd filter query

            var queryableArticles = _repository.GetAll();

            // Apply filtering
            if (!string.IsNullOrWhiteSpace(articleResource.FilterQuery)) //We decide FilterQuery is filterted against which entity 
            {
                queryableArticles = queryableArticles.Where(x => x.Author == articleResource.FilterQuery.Trim());
            }

            // Apply searching
            if (!string.IsNullOrWhiteSpace(articleResource.SearchQuery)) //We decide FilterQuery is filterted against which entity 
            {
                queryableArticles = queryableArticles.Where(x => EF.Functions.Like(x.Title, $"%{articleResource.SearchQuery.Trim()}%"));
            }

            //Apply Sorting
            if (!string.IsNullOrWhiteSpace(articleResource.OrderBy))
            {
                var mappingDictionary = _propertyMappingService.GetPropertyMapping<ReadArticle, Article>();
                queryableArticles = queryableArticles.ApplySorting<Article, ReadArticle>(articleResource.OrderBy, mappingDictionary);
            }

            var pagedEntityArticles = await queryableArticles.ToPageListAsync(articleResource.PageNumber, articleResource.PageSize);
            var mappedReadArticles = _mapper.Map<List<ReadArticle>>(pagedEntityArticles);
            var result = new PagedList<ReadArticle>(mappedReadArticles, pagedEntityArticles.TotalCount,
                                                    articleResource.PageNumber,
                                                    pagedEntityArticles.PageSize);
            return result;

        }

        public async Task<IList<User>> GetAuthorWithinRange()
        {
            var res = await _userRepository.GetPublishedArticleWithinRange(DateTime.Now.AddYears(-5), DateTime.Now);
            return res;
        }

        public async Task<IList<AuthorWithArticles>> GetAuthorWithTitle()
        {
            var res = await _userRepository.GetAuthorView();
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
            if (_userName == null || !await IsArticleOwner(_userName, article.ArticleId))
            {
                return null;
            }

            var articleToUpdate = (await _repository.FindByAsync(x => x.Id == article.ArticleId)).SingleOrDefault();
            articleToUpdate = _mapper.Map(article, articleToUpdate);
            articleToUpdate.Author = _userName;
            articleToUpdate.DateModified = DateTime.UtcNow;
            var updatedArticle = await _repository.UpdateAsync(articleToUpdate);
            return _mapper.Map<ReadArticle>(updatedArticle);
        }

        public async Task<(ReadArticle, IEnumerable<ValidationResult>)> PatchArticle(int articleId, JsonPatchArticle patchArticle)
        {
            var validationResults = new List<ValidationResult>();
            if (_userName == null || !await IsArticleOwner(_userName, articleId))
            {
                return (null, validationResults);
            }

            Expression<Func<Article, bool>> predicate = (x) => x.Id == articleId;
            var existingArticle = _repository.FindBy(predicate).SingleOrDefault();

            if (existingArticle == null)
                return (null, validationResults);

            var articleToUpdate = _mapper.Map<PatchArticle>(existingArticle);

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
        private (IEnumerable<ValidationResult>, PatchArticle) ValidateAndPatchDocument(JsonPatchArticle patchArticle,
                                                                                        PatchArticle articleToUpdate,
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

        public async Task<IList<ReadArticle>> GetArticleCollection(IEnumerable<int> articleIds)
        {
            var entityArticles = await _userRepository.GetArticleCollection(articleIds);
            var artcilesList = _mapper.Map<IList<ReadArticle>>(entityArticles);
            return artcilesList;
        }

        public List<string> ValidateSortFieldsForArticle(string orderBy)
        {            
            return _propertyMappingService.ValidateSortFields<ReadArticle, Article>(orderBy);
        }

        //Might move to IDataShaper
        public List<string> ValidateProjectionFieldsForArticle(string fields)
        {
            var invalidFields = new List<string>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return invalidFields; // No fields provided, so no invalid ones.
            }

            var properties = typeof(ReadArticle).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fieldsAfterSplit = fields.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                         .Select(f => f.Trim());

            foreach (var field in fieldsAfterSplit)
            {
                if (!properties.Any(p => p.Name.Equals(field, StringComparison.OrdinalIgnoreCase)))
                {
                    invalidFields.Add(field);
                }
            }
            return invalidFields;
        }
        
    }
}