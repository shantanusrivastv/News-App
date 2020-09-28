using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Article article)
        {
            var result = await _articleServices.CreateArticle(article);
            return Ok(result);
        }
    }
}