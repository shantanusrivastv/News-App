namespace Pressford.News.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Asp.Versioning;
    using System.Collections.Generic;
    using System;

    namespace Pressford.News.API.Controllers
    {
        /// <summary>
        /// Example controller showing how to handle multiple API versions in a SINGLE controller
        /// This demonstrates when you NEED [MapToApiVersion] attribute
        /// </summary>
        [ApiController]
        [Route("api/[controller]")]
        [ApiVersion("1.0")]
        [ApiVersion("2.0")]
        [Produces("application/json", "application/xml")]
        public class VersionExampleController : ControllerBase
        {
            #region Version 1.0 Methods

            [HttpGet("")]
            public IActionResult GetSystemStatus()
            {
                return Ok(new
                {
                    Status = "Healthy",
                    Timestamp = DateTime.Now,
                    Message = "This endpoint is available in all API versions",
                    SupportedVersions = new[] { "1.0", "2.0", "3.0" }
                });
            }

            /// <summary>
            /// Get users - Version 1.0 (Basic user info only)
            /// </summary>
            /// <returns>List of users with basic info</returns>
            [HttpGet("users")]
            [MapToApiVersion("1.0")]
            public IActionResult GetUsersV1()
            {
                var users = new List<object>
            {
                new { Id = 1, Name = "John Doe" },
                new { Id = 2, Name = "Jane Smith" }
            };

                return Ok(new
                {
                    Version = "1.0",
                    Message = "Basic user info",
                    Data = users
                });
            }

            #endregion

            #region Version 2.0 Methods

            /// <summary>
            /// Get users - Version 2.0 (Enhanced with email and additional fields)
            /// </summary>
            /// <returns>List of users with enhanced info</returns>
            [HttpGet("users")]
            [MapToApiVersion("2.0")]
            public IActionResult GetUsersV2()
            {
                var users = new List<object>
            {
                new {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    CreatedDate = DateTime.Now.AddDays(-30),
                    IsActive = true
                },
                new {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane@example.com",
                    CreatedDate = DateTime.Now.AddDays(-15),
                    IsActive = true
                }
            };

                return Ok(new
                {
                    Version = "2.0",
                    Message = "Enhanced user info with email and status",
                    Data = users,
                    TotalCount = users.Count
                });
            }


            #endregion

        }
    }
}
