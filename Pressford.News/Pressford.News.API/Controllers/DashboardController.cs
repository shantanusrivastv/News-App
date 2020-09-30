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
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [Authorize(Roles = "Publisher")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetPublisherDashboard()
        {
            ICollection<Article> publishedArticles = _dashboardService.GetPublishedArticles();
            return Ok("To do");
        }

        [Authorize(Roles = "Publisher")]
        [HttpGet]
        public async Task<IActionResult> GetUserDashboard()
        {
            return Ok("To do");
        }
    }
}