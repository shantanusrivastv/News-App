using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pressford.News.Model;
using Pressford.News.Services;

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
        public async Task<IActionResult> Get()
        {
            var articles = await _articleServices.GetAllArticles();
            return Ok(articles);
        }

        [HttpGet("{articleId:int}")]
        public async Task<IActionResult> GetById(int articleId)
        {
            var article = await _articleServices.GetSingleArticle(articleId);
            return Ok(article);
        }

        [Authorize(Roles = "Publisher")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Article article)
        {
            var result = await _articleServices.CreateArticle(article);
            return Ok(result);
        }

        [Authorize(Roles = "Publisher")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Article article)
        {
            var result = await _articleServices.UpdateArticle(article);
            return Ok(result);
        }

        [Authorize(Roles = "Publisher")]
        [HttpDelete("{articleId:int}")]
        public async Task<IActionResult> Delete(int articleId)
        {
            if (await _articleServices.RemoveArticle(articleId))
            {
                return Ok("Succesfully Deleted Resource");
            }

            //todo better error message
            return BadRequest();
        }
    }
}