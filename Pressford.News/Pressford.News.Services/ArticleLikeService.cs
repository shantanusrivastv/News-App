using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Pressford.News.Data;
using Pressford.News.Entities;

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

        public async Task<bool> LikeArticle(int articleId)
        {
            var userName = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            //Ordering is important if article is already liked then we don't want to perform another DB hit for IsValidArticle
            if (userName == null || AlreadyLiked(userName, articleId) || !IsValidArticle(articleId))
            {
                return false;
            }

            var articleLikes = new ArticleLikes()
            {
                ArticleId = articleId,
                UserName = userName
            };

            await _articleLikesRepository.AddAsync(articleLikes);

            return true;
        }

        private bool IsValidArticle(int articleId)
        {
            return _articleRepository.FindBy(x => x.Id == articleId).Any();
        }

        private bool AlreadyLiked(string userName, int articleId)
        {
            return _articleLikesRepository.FindBy(x => x.UserName == userName && x.ArticleId == articleId).Any();
        }

        public async Task<bool> UnLikeArticle(int articleId)
        {
            var userName = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;

            if (userName == null || !AlreadyLiked(userName, articleId) || !IsValidArticle(articleId))
            {
                return false;
            }

            int likeId = _articleLikesRepository.FindBy(x => x.UserName == userName && x.ArticleId == articleId)
                                                .FirstOrDefault()
                                                .LikeId;

            await _articleLikesRepository.Delete(likeId);

            return true;
        }
    }
}