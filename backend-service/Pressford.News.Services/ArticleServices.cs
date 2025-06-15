using AutoMapper;
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
using System.Security.Claims;
using System.Threading.Tasks;
using entity = Pressford.News.Entities;
using JsonPatchArticle = Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<Pressford.News.Model.PatchArticle>;

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

        public Dictionary<string, PropertyMappingValue> MappingDictionary { get; private set; } =
            new Dictionary<string, PropertyMappingValue>
            {
                { "age", new PropertyMappingValue(new[] { "DatePublished" }, revert: true) },
                { "titlewithbody", new PropertyMappingValue(new[] { "Title", "Body" }) },
                { "title", new PropertyMappingValue(new[] { "title" })}
        };

        public async Task<ReadArticle> CreateArticle(CreateArticle article)
        {
            var articleEntity = _mapper.Map<entity.Article>(article);
            articleEntity.Author = _userName ?? throw new ApplicationException("User is not logged in");
            var result = await _repository.AddAsync(articleEntity);
            return _mapper.Map<ReadArticle>(result);
        }

        public async Task<IEnumerable<ReadArticle>> CreateArticlCollection(IEnumerable<CreateArticle> articlecollection)
        {
            var articleEntity = _mapper.Map<IEnumerable<entity.Article>>(articlecollection);
            if (_userName == null)
                throw new ApplicationException("User is not logged in");

            Parallel.ForEach(articleEntity, article => article.Author = _userName);

            var articleEntitities = await _repository.AddListAsync(articleEntity);
            return _mapper.Map<IEnumerable<ReadArticle>>(articleEntitities);
        }

        public async Task<ReadArticle> GetSingleArticle(int articleId)
        {
            Expression<Func<entity.Article, bool>> predicate = (x) => x.Id == articleId;
            var result = await _repository.FindBy(predicate).SingleOrDefaultAsync();
            return (_mapper.Map<ReadArticle>(result));
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
                queryableArticles = queryableArticles.ApplySorting<Article, ReadArticle>(articleResource.OrderBy, MappingDictionary);

            }

            var pagedEntityArticles = await queryableArticles.ToPageListAsync<Article>(articleResource.PageNumber, articleResource.PageSize);
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
            articleToUpdate = _mapper.Map<UpdateArticle, entity.Article>(article, articleToUpdate);
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

            Expression<Func<entity.Article, bool>> predicate = (x) => x.Id == articleId;
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


        //todo Move outside
        public bool ValidMappingExistsFor<TSource, TDestination>(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return true;

            var orderByFields = orderBy.Split(',').Select(f => f.Trim());

            foreach (var field in orderByFields)
            {
                var fieldName = field.Split(' ')[0];
                if (!MappingDictionary.ContainsKey(fieldName)) return false;
            }

            return true;
        }

        public List<string> ValidateSortFieldsForArticle(string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy))
                return new List<string>();

            var invalids = new List<string>();

            var orderByFields = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim().ToLower());

            foreach (var field in orderByFields)
            {
                var fieldName = field.Split(' ')[0];
                if (!MappingDictionary.ContainsKey(fieldName))
                    invalids.Add(fieldName);
            }

            return invalids;
        }
    }
}