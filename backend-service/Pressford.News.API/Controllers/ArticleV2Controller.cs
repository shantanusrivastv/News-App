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
        private readonly IArticleServices _articleServices;
        private readonly IDataShaper<ReadArticle> _articleShaper;
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public ArticleV2Controller(IArticleServices articleServices,
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
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 1000)]
        //[HttpCacheValidation(MustRevalidate = false)]
        [Produces( "application/json", "application/vnd.kumar.hateoas+json")]
        //[Produces("application/json", "application/json;version=2.0", "application/vnd.kumar.hateoas+json")]
        [MapToApiVersion("2.0")]
        public IActionResult GetAllArticles([FromQuery] ArticleResourceParameters articleResource)
        {
            var artciles = GenerateArtciles();
            return Ok(artciles);
        }

        private List<ReadArticle> GenerateArtciles()
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
                    Author = "Marcus Chen",
                    DatePublished = new DateTime(2024, 2, 20),
                    DateModified = new DateTime(2024, 2, 20),  // Never modified
                    Age = 41,
                    TitleWithBody = "AI in Healthcare: Diagnostic tools using machine learning are revolutionizing patient care."
                },
                new ReadArticle
                {
                    ArticleId = 103,
                    Author = "Sophia Rodriguez",
                    DatePublished = new DateTime(2022, 11, 8),
                    DateModified = new DateTime(2023, 9, 17),
                    Age = 28,
                    TitleWithBody = "Urban Gardening Trends: How city dwellers are growing fresh produce in small spaces."
                },
                new ReadArticle
                {
                    ArticleId = 104,
                    Author = "David Kim",
                    DatePublished = new DateTime(2020, 3, 12),
                    DateModified = new DateTime(2024, 6, 1),  // Recently updated
                    Age = 52,
                    TitleWithBody = "Blockchain Beyond Cryptocurrency: Supply chain applications are increasing transparency."
                },
                new ReadArticle
                {
                    ArticleId = 105,
                    Author = "Aisha Patel",
                    DatePublished = new DateTime(2024, 4, 5),
                    DateModified = new DateTime(2024, 7, 10),  // Recently modified
                    Age = 29,
                    TitleWithBody = "Neuroplasticity and Learning: New research shows how adults can still master new skills."
                }
            };
        }
    }
}

        
    
