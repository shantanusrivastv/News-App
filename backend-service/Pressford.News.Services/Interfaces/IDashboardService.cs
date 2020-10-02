using System.Collections.Generic;
using System.Threading.Tasks;
using Pressford.News.Model;

namespace Pressford.News.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<List<Article>> GetPublishedArticles();

        Task<List<Article>> GetAllPublishedArticle();
    }
}