using Microsoft.AspNetCore.JsonPatch;
using Pressford.News.Model;
using Pressford.News.Model.Helpers;
using Pressford.News.Model.ResourceParameters;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Pressford.News.Services.Interfaces
{
	public interface IArticleServices
	{
		Task<ReadArticle> CreateArticle(CreateArticle article);

		Task<PagedList<ReadArticle>> GetAllArticles(ArticleResourceParameters articleResource);

		Task<ReadArticle> GetSingleArticle(int articleId);

		Task<ReadArticle> UpdateArticle(UpdateArticle article);

		Task<bool> RemoveArticle(int articleId);

		Task<(ReadArticle, IEnumerable<ValidationResult>)> PatchArticle(int articleId, JsonPatchDocument<PatchArticle> patchDoc);

		//Todo refactor to its own svc 
        Task<IEnumerable<ReadArticle>> CreateArticlCollection(IEnumerable<CreateArticle> articlecollection);
        Task<IList<ReadArticle>> GetArticleCollection(IEnumerable<int> articleIds);
    }
}