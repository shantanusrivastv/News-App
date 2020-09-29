using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pressford.News.Services;

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
        public IActionResult LikeArticle(int articleId)
        {
            var user = _likeService.LikeArticle(articleId);

            if (!user)
                return BadRequest();
            return Ok("Article Like was successfully updated");
        }

        //[Authorize]
        [HttpDelete("[action]/{articleId:int}")]
        public IActionResult UnlikeArticle(int articleId)
        {
            var user = _likeService.UnLikeArticle(articleId);

            if (!user)
                return StatusCode(StatusCodes.Status500InternalServerError);
            return Ok("Article UnLike was successfully updated");
        }
    }
}