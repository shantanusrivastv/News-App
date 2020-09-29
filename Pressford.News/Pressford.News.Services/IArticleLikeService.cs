using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Services
{
    public interface IArticleLikeService
    {
        bool LikeArticle(int articleId);

        bool UnLikeArticle(int articleId);
    }
}