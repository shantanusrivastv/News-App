using Microsoft.AspNetCore.Mvc;
using Pressford.News.Model;
using System;
using System.Collections.Generic;

namespace Pressford.News.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class RootController : ControllerBase
    {
        [HttpGet("/api", Name = "GetApiRoot")]
        [Produces("application/json", "application/vnd.kumar.hateoas+json")]
        public IActionResult GetApiRoot()
        {
            // create links for root
            var links = new List<LinkDto>();

            links.Add(new(Url.Link("GetApiRoot", new { }),
              "self",
              "GET"));

            links.Add(
              new(Url.Link("GetArticles", new { }),
              "get-articles",
              "GET"));

            links.Add(
              new(Url.Link("CreateArticle", new { }),
              "create_article",
              "POST"));

            Response.ContentType = "application/vnd.kumar.hateoas+json";

            return Ok(links);
        }
    }
}
