using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pressford.News.Model;
using Pressford.News.Services.Interfaces;

namespace Pressford.News.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] Credentials credentials)
        {
            var user = _userService.Authenticate(credentials);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });
            return Ok(user);
        }
    }
}