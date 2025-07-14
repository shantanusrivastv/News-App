using Asp.Versioning;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Pressford.News.Entities;
using Pressford.News.Model;
using Pressford.News.Model.ResourceParameters;
using Pressford.News.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Pressford.News.API.Controllers
{
    [ApiController]
    [Route("api/Article")]
    [ApiVersion("1.0")]
    //[ResponseCache(CacheProfileName = "240SecondsCacheProfile")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public)]
    [HttpCacheValidation(MustRevalidate = true)]
    [Produces("application/json", "application/xml")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleServices _articleServices;
        private readonly IDataShaper<ReadArticle> _articleShaper;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public ArticleController(IArticleServices articleServices,
                                 IDataShaper<ReadArticle> articleShaper,
                                 ProblemDetailsFactory problemDetailsFactory)
        {
            _articleServices = articleServices;
            _articleShaper = articleShaper;
            _problemDetailsFactory = problemDetailsFactory;
        }

        /// <summary>
        /// Get all articles
        /// </summary>
        /// <returns></returns>
        //[ResponseCache(Duration = 120)]
        [HttpGet, HttpHead(Name = "GetArticles")] // GET /api/article
        [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 1000)]
        [HttpCacheValidation(MustRevalidate = false)]
        [Produces("application/json", "application/vnd.kumar.hateoas+json")]
        public async Task<IActionResult> GetAllArticles([FromQuery] ArticleResourceParameters articleResource)
        {
            var invalids = _articleServices.ValidateSortFieldsForArticle(articleResource.OrderBy);
            if (invalids.Any())
            {
                return BadRequest($"Invalid sort fields provided: {string.Join(", ", invalids)}");
            }

            //todo move both invalid into single method /svc
            invalids = _articleServices.ValidateProjectionFieldsForArticle(articleResource.Fields);
            if (invalids.Any())
            {
                return BadRequest($"Invalid projection fields provided: {string.Join(", ", invalids)}");
            }

            var articles = await _articleServices.GetAllArticles(articleResource);
            this.AddPaginationHeaders(articles, articleResource, "GetArticles");
            IEnumerable<ExpandoObject> shapedArticle = null;
            if (!String.IsNullOrWhiteSpace(articleResource.Fields))
            {
                shapedArticle = _articleShaper.ShapeData(articles, articleResource.Fields);
               
            }

            //todo we are currently always doing shapeDat
            if (Request.Headers["Accept"].ToString().Contains("application/vnd.kumar.hateoas+json"))
            {
                var shapedArticleWithLinks = _articleShaper.ShapeData(articles, articleResource.Fields).Select(article =>
                {
                    var articleAsDictionary = article as IDictionary<string, object?>;
                    var perArticleLink = this.GenerateLinksPerResource("Article", (int)articleAsDictionary["ArticleId"]);
                    articleAsDictionary.Add("links", perArticleLink);
                    return articleAsDictionary;
                });

                // create Collection links
                var links = this.GeneratePaginationLinks(articleResource, "GetArticles",
                                                          articleResource,
                                                          articles.HasNextPage,
                                                          articles.HasPreviousPage);

                var linkedCollectionResource = new
                {
                    value = shapedArticleWithLinks,
                    ParentLink = links
                };

                Response.ContentType = "application/vnd.kumar.hateoas+json";
                return Ok(linkedCollectionResource);
            }

            if (shapedArticle?.Count() > 1 )
            {
                return Ok(shapedArticle);
            }
            else
            {
                return Ok(articles);
            }          

        }

        [HttpGet("{articleId:int}", Name = "GetArticle")] // GET /api/article/1
        [Produces("application/json", "application/vnd.kumar.hateoas+json")]
        public async Task<IActionResult> GetSingleArticle(int articleId, string fields = "")
        {
            var invalids = _articleServices.ValidateProjectionFieldsForArticle(fields);
            if (invalids.Any())
            {
                return BadRequest(
                _problemDetailsFactory.CreateProblemDetails(HttpContext,
                    statusCode: 400,
                    detail: $"Invalid projection fields provided: {string.Join(", ", invalids)}"));
            }

            var article = await _articleServices.GetSingleArticle(articleId);
            if (article == null)
                return NotFound();

            ExpandoObject shapedArticle = null;

            if (!string.IsNullOrWhiteSpace(fields))
            {
                shapedArticle = _articleShaper.ShapeData(article, fields);
            }


            //var paginationLinks = this.GeneratePaginationLinks("GetArticles",
            //    new { pageNumber, pageSize }, metadata);

            // Add collection-specific links
            //var collectionLinks = this.GenerateLink("Article",

            // var allLinks = paginationLinks.Concat(collectionLinks).ToList();

            if (Request.Headers["Accept"].ToString().Contains("application/vnd.kumar.hateoas+json"))
            {

                var links = this.GenerateLinksForSinlgeResource("Article", articleId, fields);

                var shapedArtcile = _articleShaper.ShapeData(article, fields) as IDictionary<string, object>;
                shapedArtcile.Add("links", links);

                //var response = new { article, _links = links };
                Response.ContentType = "application/vnd.kumar.hateoas+json";
                return Ok(shapedArtcile);
            }

            if(shapedArticle is not null)
            {
                return Ok(shapedArticle);
            }

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
        [HttpPost(Name = "CreateArticle")]
        [ProducesResponseType(typeof(ReadArticle), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateNewArticle([FromBody] CreateArticle article)
        {
            var result = await _articleServices.CreateArticle(article);
            return CreatedAtRoute("GetArticle", new { articleId = result.ArticleId }, result);
        }

        [Authorize(Roles = "Publisher")]
        [HttpPut(Name = "UpdateArticle")]
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
        [HttpPatch(Name = "PatchArticle")]
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
        [HttpDelete("{articleId:int}", Name = "DeleteArticle")]
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