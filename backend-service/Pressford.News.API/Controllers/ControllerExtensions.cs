using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using Pressford.News.Model;
using Pressford.News.Model.Helpers;
using System.Text.Json;

namespace Pressford.News.API.Controllers
{
    public static class ControllerExtensions
    {
        public static void AddPaginationHeaders<T>(this ControllerBase controller,
            PagedList<T> pagedList,
            object routeParameters,
            string routeName)
        {
            var metadata = new PaginationMetadata
            {
                TotalCount = pagedList.TotalCount,
                PageSize = pagedList.PageSize,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                HasPreviousPage = pagedList.HasPreviousPage,
                HasNextPage = pagedList.HasNextPage,
                PreviousPageLink = pagedList.HasPreviousPage ? controller.GeneratePageLink(routeName, routeParameters, pagedList.CurrentPage -1)
                                                              : null,
                NextPageLink = pagedList.HasNextPage ? controller.GeneratePageLink(routeName, routeParameters, pagedList.CurrentPage + 1)
                                                         : null
            };


            controller.Response?.Headers?.Add("X-Pagination",
            JsonSerializer.Serialize(metadata, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }));
        }
    

        private static string? GeneratePageLink(this ControllerBase controller, string routeName, object routeParameters, int newPageNumber)
        {
            var routeValues = new RouteValueDictionary(routeParameters)
            {
                ["pageNumber"] = newPageNumber
            };

            return controller.Url.Link(routeName, routeValues);
        }
    }
}
