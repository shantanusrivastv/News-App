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

		//todo maybe we can toggle it
		[Authorize]
		[HttpPut("[action]/{articleId:int}")]
		public async Task<IActionResult> LikeArticle(int articleId)
		{
			var user = await _likeService.LikeArticle(articleId);
			if (!user)
                return BadRequest(new { success = false, message = "Unable to process like request" });
            return Ok(new { success = true, message = "Article unlike status updated successfully" });
        }

		[Authorize]
		[HttpPut("[action]/{articleId:int}")]
		public async Task<IActionResult> UnlikeArticle(int articleId)
		{
			var user = await _likeService.UnLikeArticle(articleId);
			if (!user)
                return BadRequest(new { success = false, message = "Unable to process unlike request" });
            return Ok(new { success = true, message = "Article unlike status updated successfully" });
        }
    }
}