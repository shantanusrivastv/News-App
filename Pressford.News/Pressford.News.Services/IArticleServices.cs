using System.Collections.Generic;
using System.Threading.Tasks;
using Pressford.News.Model;

namespace Pressford.News.Services
{
    public interface IArticleServices
    {
        Task<Article> CreateArticle(Article article);
        Task<IList<Article>> GetAllArticles();
    }
}