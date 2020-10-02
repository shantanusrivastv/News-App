using System.Collections.Generic;
using System.Threading.Tasks;
using Pressford.News.Model;

namespace Pressford.News.Services.Interfaces
{
    public interface IArticleServices
    {
        Task<Article> CreateArticle(Article article);

        Task<IList<Article>> GetAllArticles();

        Task<Article> GetSingleArticle(int articleId);

        Task<Article> UpdateArticle(Article article);

        Task<bool> RemoveArticle(int articleId);
    }
}