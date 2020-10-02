using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pressford.News.Services.Interfaces;

namespace Pressford.News.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ArticleLikesController : ControllerBase
    {
        private readonly IArticleLikeService _likeService;

        public ArticleLikesController(IArticleLikeService likeService)
        {
            _likeService = likeService;
        }

        [Authorize]
        [HttpPost("[action]/{articleId:int}")]
        public async Task<IActionResult> LikeArticle(int articleId)
        {
            var user = await _likeService.LikeArticle(articleId);
            if (!user)
                return BadRequest();
            return Ok("Article Like was successfully updated");
        }

        [Authorize]
        [HttpDelete("[action]/{articleId:int}")]
        public async Task<IActionResult> UnlikeArticle(int articleId)
        {
            var user = await _likeService.UnLikeArticle(articleId);
            if (!user)
                return BadRequest();
            return Ok("Article UnLike was successfully updated");
        }
    }
}