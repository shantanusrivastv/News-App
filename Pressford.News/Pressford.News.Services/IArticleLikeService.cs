using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Services
{
    public interface IArticleLikeService
    {
        Task<bool> LikeArticle(int articleId);

        Task<bool> UnLikeArticle(int articleId);
    }
}