using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Pressford.News.Model;
using Pressford.News.Model.ResourceParameters;
using Pressford.News.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Pressford.News.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json", "application/xml")]
	//TOOD: Add versioning of the API
	public class ArticleController : ControllerBase
	{
		private readonly IArticleServices _articleServices;

		public ArticleController(IArticleServices articleServices)
		{
			_articleServices = articleServices;
		}

		/// <summary>
		/// Get all articles
		/// </summary>
		/// <returns></returns>
		[HttpGet, HttpHead(Name = "GetArticles")] // GET /api/article
        public async Task<IActionResult> GetAllArticles([FromQuery] ArticleResourceParameters articleResource)
		{
			var invalids = _articleServices.ValidateSortFieldsForArticle(articleResource.OrderBy);
			if (invalids.Any())
			{
				return BadRequest($"Invalid sort fields: {string.Join(", ", invalids)}");
			}

			var articles = await _articleServices.GetAllArticles(articleResource);
            this.AddPaginationHeaders(articles, articleResource, "GetArticles");
            return Ok(articles);
		}

		[HttpGet("{articleId:int}", Name = "GetArticle")] // GET /api/article/1
        public async Task<IActionResult> GetSingleArticle(int articleId)
		{
			var article = await _articleServices.GetSingleArticle(articleId);
			if (article == null)
				return NotFound();
			return Ok(article);
		}

		/// <summary>
		/// Create a new article
		/// </summary>
		/// <param name="article"></param>
		/// <returns>Newly created article</returns>
		/// <remarks>
		/// Sample request:
		///
		///     POST /Article
		///     {
		///        "title": "New title",
		///        "body": "New body"
		///     }
		/// </remarks>
		/// <response code="201">Returns the newly created item</response>
		/// <response code="400">For Invalid Input</response>
		[Authorize(Roles = "Publisher")]
		[HttpPost]
		[ProducesResponseType(typeof(ReadArticle), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
		[Consumes("application/json")]
		public async Task<IActionResult> CreateNewArticle([FromBody] CreateArticle article)
		{
			var result = await _articleServices.CreateArticle(article);
			return CreatedAtRoute("GetArticle", new { articleId = result.ArticleId }, result);
		}

		[Authorize(Roles = "Publisher")]
		[HttpPut]
		public async Task<IActionResult> UpdateArticle([FromBody] UpdateArticle article)
		{
			var result = await _articleServices.UpdateArticle(article);
			if (result == null)
				return Unauthorized("Either the article does not exist or user does not have required privileges ");
			return Ok(result);
		}

		//It is suggested to passing the id in the URL,I am not doing it since I don't want to change
		// since it matches with existing Update method need to think again
		[Authorize(Roles = "Publisher")]
		[HttpPatch]
		[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> PatchArticle([FromBody] JsonPatchDocument<PatchArticle> patchArticle)
		{
			var idOperation = patchArticle.Operations.FirstOrDefault(op => op.path.Equals("/articleId", StringComparison.OrdinalIgnoreCase));
			if (idOperation == null || !int.TryParse(idOperation.value?.ToString(), out int id))
				return BadRequest("Article ID not provided or invalid");

			var (result, validationErrors) = await _articleServices.PatchArticle(id, patchArticle);

			if (validationErrors.Any())
			{
				var errorsByField = validationErrors
										.GroupBy(error => error.MemberNames.Any() ? error.MemberNames.First() : "General")
										.ToDictionary(
											group => group.Key,
											group => group.Select(error => error.ErrorMessage).ToArray()
										);

				var validationProblem = new ValidationProblemDetails
				{
					Detail = "Please refer to the  property errors for additional details",
					Errors = errorsByField
				};

				return ValidationProblem(validationProblem);
			}

			if (result == null)
				return Unauthorized("Either the article does not exist or user does not have required privileges ");

			return Ok(result);
		}

		[Authorize(Roles = "Publisher")]
		[HttpDelete("{articleId:int}")]
		public async Task<IActionResult> DeleteArticle(int articleId)
		{
			if (await _articleServices.RemoveArticle(articleId))
			{
				return Accepted("Successfully Deleted Resource");
			}
			return Unauthorized("Either the article does not exist or user does not have required privileges ");
		}

		[HttpOptions]
		public IActionResult GetArticleOptions()
		{
			Response.Headers.Add("Allow", "GET,HEAD,POST,DELETE,OPTIONS");
			return Ok();
		}

        //public override ActionResult ValidationProblem(
        //    [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        //{
        //    var options = HttpContext.RequestServices
        //        .GetRequiredService<IOptions<ApiBehaviorOptions>>();

        //    return (ActionResult)options.Value
        //        .InvalidModelStateResponseFactory(ControllerContext);
        //}
    }
}