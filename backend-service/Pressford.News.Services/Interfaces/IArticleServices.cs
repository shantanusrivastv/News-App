using System.Collections.Generic;
using System.Threading.Tasks;
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
	}
}