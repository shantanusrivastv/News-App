using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pressford.News.API.ModelBinder;
using Pressford.News.Model;
using Pressford.News.Services;
using Pressford.News.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArticleBase = Pressford.News.Model.ArticleBase;

namespace Pressford.News.API.Controllers
{
    [Route("api/articlecollection")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    public class ArticleCollectionController : ControllerBase
    {
        private readonly IArticleServices _articleServices;

        public ArticleCollectionController(IArticleServices articleServices)
        {
            _articleServices = articleServices;
        }

        [HttpGet(Name = "GetArticleCollection")]
        public async Task<IActionResult> GetArticleCollection([FromQuery][ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> articleIds)
        {
            var articles = await _articleServices.GetArticleCollection(articleIds);
            if (articles == null )
                return NotFound();

            if (articles.Count != articleIds.Count())
                return BadRequest( new { error = "Request contains one or more invalid article id" });

            return Ok(articles);
        }


        [Authorize(Roles = "Publisher")]
        [HttpPost]
        [ProducesResponseType(typeof(IList<ReadArticle>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateNewArticle(IEnumerable<CreateArticle> articlelist)
        {
            var articles =  await _articleServices.CreateArticlCollection(articlelist);
            var articleIds = string.Join(",", articles.Select(a => a.ArticleId));
            return CreatedAtRoute("GetArticleCollection", new { articleIds}, articles);
        }
    }
}
