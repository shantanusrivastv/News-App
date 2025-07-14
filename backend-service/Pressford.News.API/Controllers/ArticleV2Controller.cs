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
    [ApiVersion("2.0")]
    [Produces("application/json", "application/xml")]
    public class ArticleV2Controller : ControllerBase
    {
        /// <summary>
        /// Get all articles ensure to disable cache in dev tools
        /// </summary>
        /// <returns>ReadArticle</returns>
        [HttpGet, HttpHead(Name = "GetArticles")] // GET /api/article
        [Produces( "application/json", "application/vnd.kumar.hateoas+json")]
        //[Produces("application/json", "application/json;version=2.0", "application/vnd.kumar.hateoas+json")]
        public IActionResult GetAllArticles()
        {
            var artciles = GenerateArtciles();
            return Ok(artciles);
        }

        private static List<ReadArticle> GenerateArtciles()
        {
            return new List<ReadArticle>
            {
                new ReadArticle
                {
                    ArticleId = 101,
                    Author = "Version 2 Response",
                    DatePublished = new DateTime(2023, 5, 15),
                    DateModified = new DateTime(2024, 1, 10),
                    Age = 34,
                    TitleWithBody = "The Future of Renewable Energy: Solar innovations are changing how we power our homes and cities."
                },
                new ReadArticle
                {
                    ArticleId = 102,
                    Author = "Version 2 Marcus Chen",
                    DatePublished = new DateTime(2024, 2, 20),
                    DateModified = new DateTime(2024, 2, 20),  // Never modified
                    Age = 41,
                    TitleWithBody = "AI in Healthcare: Diagnostic tools using machine learning are revolutionizing patient care."
                }
            };
        }
    }
}

        
    
