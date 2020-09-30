using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            var publishedArticles = await _dashboardService.GetPublishedArticles();
            return Ok(publishedArticles);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUserDashboard()
        {
            var publishedArticles = await _dashboardService.GetAllPublishedArticle();
            return Ok(publishedArticles);
        }
    }
}