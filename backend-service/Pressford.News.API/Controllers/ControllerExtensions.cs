using Azure.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json.Linq;
using Pressford.News.Model;
using Pressford.News.Model.Helpers;
using Pressford.News.Model.ResourceParameters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.CompilerServices;
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

        // Generate multiple links for a resource
        public static ICollection<LinkDto> GenerateResourceLinks<T>(this ControllerBase controller,
            T resource,
            object? routeParameters = null) where T : class
        {
            var links = new List<LinkDto>();

            var resourceType = typeof(T);
            var resourceName = resourceType.Name.Replace("Read", "").Replace("Model", "");
            var routePrefix = $"{resourceName}"; // Articles, Users, etc.

            // Assuming standard REST naming conventions
            //var routePrefix =  $"{resourceName}s"; // Articles, Users, etc.


            // Self link
            var selfLink = controller.GenerateLink($"Get{resourceName}", routeParameters, "self");
            if (selfLink != null) links.Add(selfLink);

            // Collection link
            var collectionLink = controller.GenerateLink($"Get{routePrefix}s", null, "collection", "GET");
            if (collectionLink != null) links.Add(collectionLink);

            // Edit link (assuming we have update capability)
            var editLink = controller.GenerateLink($"Update{resourceName}", routeParameters, "edit", "PUT");
            if (editLink != null) links.Add(editLink);

            // Delete link
            var deleteLink = controller.GenerateLink($"Delete{resourceName}", routeParameters, "delete", "DELETE");
            if (deleteLink != null) links.Add(deleteLink);

            return links;
        }

        public static ICollection<LinkDto> GenerateLinksPerResource(this ControllerBase controller, string resourceName,
                                                                        int routeParameters)
        {
            var links = new List<LinkDto>();


            var routeParams = new Dictionary<string, object>();
            routeParams["articleId"] = routeParameters;



            // Self link
            var selfLink = controller.GenerateLink($"Get{resourceName}", routeParams, "self");
            if (selfLink != null) links.Add(selfLink);

            // Collection link
            var collectionLink = controller.GenerateLink($"Get{resourceName}s", null, "collection", "GET");
            if (collectionLink != null) links.Add(collectionLink);

            // Edit link (assuming we have update capability)
            var editLink = controller.GenerateLink($"Update{resourceName}", routeParams, "edit", "PUT");
            if (editLink != null) links.Add(editLink);

            // Delete link
            var deleteLink = controller.GenerateLink($"Delete{resourceName}", routeParams, "delete", "DELETE");
            if (deleteLink != null) links.Add(deleteLink);

            return links;
        }

        public static ICollection<LinkDto> GenerateLinksForSinlgeResource(this ControllerBase controller, string resourceName,
                                                                        int routeParameters,
                                                                        string fields = "")
        {
            var links = new List<LinkDto>();

            var routeParams = new Dictionary<string, object>();
            routeParams["articleId"] = routeParameters;

            // Self link
            var routeParamsForGet = new Dictionary<string, object>();
            routeParams["articleId"] = routeParameters;
            if (!string.IsNullOrWhiteSpace(fields))
            {
                routeParamsForGet.Add("fields", fields);
            }

            var selfLink = controller.GenerateLink($"Get{resourceName}", routeParamsForGet, "self");
            if (selfLink != null) links.Add(selfLink);

            // Collection link
            var collectionLink = controller.GenerateLink($"Get{resourceName}s", null, "collection", "GET");
            if (collectionLink != null) links.Add(collectionLink);

            // Edit link (assuming we have update capability)
            var editLink = controller.GenerateLink($"Update{resourceName}", routeParams, "edit", "PUT");
            if (editLink != null) links.Add(editLink);

            // Delete link
            var deleteLink = controller.GenerateLink($"Delete{resourceName}", routeParams, "delete", "DELETE");
            if (deleteLink != null) links.Add(deleteLink);


            return links;
        }

        // Generate collection links (for lists of resources)
        public static ICollection<LinkDto> GenerateCollectionLinks(this ControllerBase controller,
            string resourceName,
            object? queryParameters = null)
        {
            var links = new List<LinkDto>();
            var routePrefix = $"{resourceName}s";

            // Self link for collection
            var selfLink = controller.GenerateLink($"Get{routePrefix}", queryParameters, "self");
            if (selfLink != null) links.Add(selfLink);

            // Create new resource link
            var createLink = controller.GenerateLink($"Create{resourceName}", null, "create", "POST");
            if (createLink != null) links.Add(createLink);

            // Search link (if applicable)
            var searchLink = controller.GenerateLink($"Search{routePrefix}", null, "search", "GET");
            if (searchLink != null) links.Add(searchLink);

            return links;
        }


        // Enhanced pagination with HATEOAS
        public static ICollection<LinkDto> GeneratePaginationLinks(this ControllerBase controller,
            object routeParameters,
            string routeName,
            ArticleResourceParameters metadata,
            bool hasPrevPage = false,
            bool hasNextPage = false)
        {
            var links = new List<LinkDto>();


            //var prevPageLink = hasPrevPage ? controller.GeneratePageLink("GetArticles", routeParameters, CurrentPage - 1)
            //                                    : null;

            //var anotherPageLink = hasPrevPage ? controller.GenerateLink("GetArticles", routeParameters, "Prev Page", "GET");
            //                                                : null;

            //var NextPageLink = hasNxtPage ? controller.GeneratePageLink("GetArticles", routeParameters, CurrentPage + 1)
            //                                             : null;

            // First page
            if (metadata.PageNumber > 1)
            {
                var firstLink = controller.GeneratePageLink(routeName, routeParameters, 1);
                if (firstLink != null)
                    links.Add(new LinkDto(firstLink, "first", "GET"));
            }

            // Previous page
            if (hasPrevPage)
            {
                var prevLink = controller.GeneratePageLink(routeName, routeParameters, metadata.PageNumber - 1);
                if (prevLink != null)
                    links.Add(new LinkDto(prevLink, "prev", "GET"));
            }

            // Self (current page)
            var selfLink = controller.GeneratePageLink(routeName, routeParameters, metadata.PageNumber);
            if (selfLink != null)
                links.Add(new LinkDto(selfLink, "self", "GET"));

            // Next page
            if (hasNextPage)
            {
                var nextLink = controller.GeneratePageLink(routeName, routeParameters, metadata.PageNumber + 1);
                if (nextLink != null)
                    links.Add(new LinkDto(nextLink, "next", "GET"));
            }

            // todo Last page
            //if (metadata.PageNumber < metadata.TotalPages)
            //{
            //    var lastLink = controller.GeneratePageLink(routeName, routeParameters, metadata.TotalPages);
            //    if (lastLink != null)
            //    links.Add(new LinkDto(lastLink, "last", "GET"));

            //}

            return links;
        }


        // Generate a single link resource
        public static LinkDto? GenerateLink(this ControllerBase controller,
            string routeName,
            object? routeParameters = null,
            string rel = "self",
            string method = "GET")
        {
            //var url = routeParameters != null
            //    ? controller.Url.Link(routeName, routeParameters)
            //    : controller.Url.Link(routeName, new { });

            var url = controller.Url.Link(routeName, routeParameters);

            if (string.IsNullOrEmpty(url))
                return null;

            return new LinkDto(url, rel, method);
        }

        // Create HATEOAS response
        public static List<LinkDto> CreateHateoasResponse<T>(this ControllerBase controller,
            T data,
            ICollection<LinkDto>? additionalLinks = null) where T : class
        {
            ////if (!controller.AcceptsHateoas())
            //{
            //    // Return standard JSON if client doesn't accept HATEOAS
            //    return new OkObjectResult(data);
            //}

            var links = new List<LinkDto>();
            var routeValues = controller.RouteData.Values;

            var resourceNameSec = typeof(T).Name.Replace("Read", "").Replace("Model", "");
            var resourceIdentifier = $"{resourceNameSec}Id";
            var routeParams = new Dictionary<string, object>();
            if (routeValues != null && routeValues.Values.Count > 0)
            {
                if (routeValues.TryGetValue(resourceIdentifier, out var routeParameter))
                {
                    routeParams[resourceIdentifier] = routeParameter;
                }

            }

            // Add resource-specific links
            if (data is IEnumerable && !(data is string))
            {
                // Handle collections
                var resourceName = typeof(T).GetGenericArguments().FirstOrDefault()?.Name ?? "Resource";
                links.AddRange(controller.GenerateCollectionLinks(resourceName.Replace("Resource", "").Replace("Model", "")));
            }
            else
            {
                // Handle single resources
                links.AddRange(controller.GenerateResourceLinks(data, routeParams));
            }

            // Add any additional links provided
            if (additionalLinks != null)
                links.AddRange(additionalLinks);

            //var hateoasResponse = new HateoasResource<T>
            //{
            //    Data = data,
            //    Links = links
            //};



            // Set the custom content type
            //controller.Response.ContentType = "application/vnd.kumar.hateoas+json";

            //return new OkObjectResult(hateoasResponse);
            return links;
        }
    }
}
