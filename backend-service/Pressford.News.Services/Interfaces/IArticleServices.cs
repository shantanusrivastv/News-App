using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Pressford.News.Model;

namespace Pressford.News.Services.Interfaces
{
	public interface IArticleServices
	{
		Task<ReadArticle> CreateArticle(ArticleBase article);

		Task<IList<ReadArticle>> GetAllArticles();

		Task<ReadArticle> GetSingleArticle(int articleId);

		Task<ReadArticle> UpdateArticle(UpdateArticle article);

		Task<bool> RemoveArticle(int articleId);

		Task<(ReadArticle, IEnumerable<ValidationResult>)> PatchArticle(int articleId, JsonPatchDocument<UpdateArticle> patchDoc);

		//Todo refactor to its own svc 
        Task<IEnumerable<ReadArticle>> CreateArticlCollection(IEnumerable<ArticleBase> articlecollection);
        Task<IList<ReadArticle>> GetArticleCollection(IEnumerable<int> articleIds);
    }
}