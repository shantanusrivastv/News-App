using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Pressford.News.Data;
using Pressford.News.Entities;
using System.Threading.Tasks;

namespace Pressford.News.Services
{
    public class ArticleLikeService : IArticleLikeService
    {
        private IRepository<Article> _articleRepository;
        private IRepository<ArticleLikes> _articleLikesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArticleLikeService(IRepository<Article> articleRepository, IRepository<ArticleLikes> articleLikesRepository, IHttpContextAccessor httpContext)
        {
            _articleRepository = articleRepository;
            _articleLikesRepository = articleLikesRepository;
            _httpContextAccessor = httpContext;
        }

        public bool LikeArticle(int articleId)
        {
            var userName = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            //Ordering is important if article is already liked then we don't want to perform another DB hit for IsValidArticle
            if (userName == null || HasAlreadyLikedByUser(userName) || !IsValidArticle(articleId))
            {
                return false;
            }

            var articleLikes = new ArticleLikes()
            {
                ArticleId = articleId,
                UserName = userName
            };

            Task.WaitAll(_articleLikesRepository.AddAsync(articleLikes));

            return true;
        }

        private bool IsValidArticle(int articleId)
        {
            return _articleRepository.FindBy(x => x.Id == articleId).Any();
        }

        private bool HasAlreadyLikedByUser(string userName)
        {
            return _articleLikesRepository.FindBy(x => x.UserName == userName).Any();
        }

        public bool UnLikeArticle(int articleId)
        {
            throw new System.NotImplementedException();
        }
    }
}