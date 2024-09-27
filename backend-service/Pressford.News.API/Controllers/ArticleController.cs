﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Pressford.News.Model;
using Pressford.News.Services.Interfaces;

namespace Pressford.News.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ArticleController : ControllerBase
	{
		private readonly IArticleServices _articleServices;

		public ArticleController(IArticleServices articleServices)
		{
			_articleServices = articleServices;
		}

		[HttpGet]
		public async Task<IActionResult> GetAllArticles()
		{
			var articles = await _articleServices.GetAllArticles();
			return Ok(articles);
		}

		[HttpGet("{articleId:int}", Name = "GetArticle")]
		public async Task<IActionResult> GetSingleArticle(int articleId)
		{
			var article = await _articleServices.GetSingleArticle(articleId);
			if (article == null)
				return NotFound();
			return Ok(article);
		}

		[Authorize(Roles = "Publisher")]
		[HttpPost]
		public async Task<IActionResult> CreateNewArticle([FromBody] ArticleBase article)
		{
			var result = await _articleServices.CreateArticle(article);
			return CreatedAtRoute("GetArticle", new { articleId = result.Id }, result);
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
		// since it matches with eixsting Update method need to think again
		[Authorize(Roles = "Publisher")]
		[HttpPatch]
		public async Task<IActionResult> PatchArticle([FromBody] JsonPatchDocument<UpdateArticle> patchArticle)
		{
			var idOperation = patchArticle.Operations.FirstOrDefault(op => op.path.Equals("/Id", StringComparison.OrdinalIgnoreCase));
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
			Response.Headers.Add("Allow", "GET,OPTIONS,POST");
			return Ok();
		}
	}
}