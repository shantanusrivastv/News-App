using System.Threading.Tasks;

namespace Pressford.News.Services.Interfaces
{
    public interface IArticleLikeService
    {
        Task<bool> LikeArticle(int articleId);

        Task<bool> UnLikeArticle(int articleId);
    }
}