using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Pressford.News.API
{
    public class CustomTagOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Get the controller name from the action descriptor
            var controllerName = context.ApiDescription.ActionDescriptor.RouteValues["controller"];

            // Clear existing tags to remove default controller-derived tags
            operation.Tags.Clear();

            // Assign custom tags based on controller name or other logic
            //if (controllerName == "Article") // For ArticleController (V1)
            //{
            //    operation.Tags.Add(new OpenApiTag { Name = "Articles API - Version 1" });
            //}
             if (controllerName == "ArticleV") // For ArticleV2Controller (V2)
            {
                operation.Tags.Add(new OpenApiTag { 
                    Name = "Articles Version 2", 
                    Description = "Disable Cache from the Dev Tools"
                
                });
            }
            else
            {
                // Fallback for other controllers, or if you want to keep their default names
                operation.Tags.Add(new OpenApiTag { Name = controllerName });
            }
        }
    }
}
