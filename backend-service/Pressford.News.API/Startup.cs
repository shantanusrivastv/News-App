using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Pressford.News.Services.Dependencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Pressford.News.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Security Vulnerability not fit for production
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddControllers(setupAction =>
            {
                // Return 406 Not Acceptable if the client requests a format the server does not support
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.RespectBrowserAcceptHeader = true;
                //setupAction.Conventions.Add(new ProducesAttributeConvention());
                setupAction.CacheProfiles.Add("240SecondsCacheProfile",
                                              new() { Duration = 240 });

            }).AddNewtonsoftJson()
              .AddXmlDataContractSerializerFormatters()
			  .ConfigureApiBehaviorOptions( setupAction =>
              {
                  setupAction.InvalidModelStateResponseFactory = context =>
                  {
                      // create a validation problem details object
                      var problemDetailsFactory = context.HttpContext.RequestServices
                          .GetRequiredService<ProblemDetailsFactory>();

                      var validationProblemDetails = problemDetailsFactory
                          .CreateValidationProblemDetails(
                              context.HttpContext,
                              context.ModelState);

                      // add additional info not added by default
                      validationProblemDetails.Detail =
                          "See the errors field for details.";
                      validationProblemDetails.Instance =
                          context.HttpContext.Request.Path;

                      // report invalid model state responses as validation issues
                      validationProblemDetails.Type =
                          "https://PressfordNews.com/modelvalidationproblem";
                      validationProblemDetails.Status =
                          StatusCodes.Status422UnprocessableEntity;
                      validationProblemDetails.Title =
                          "One or more validation errors occurred.";

                      return new UnprocessableEntityObjectResult(
                          validationProblemDetails)
                      {
                          ContentTypes = { "application/problem+json" }
                      };
                  };
              });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                                    new HeaderApiVersionReader("Api-Version"),
                                    new MediaTypeApiVersionReader("version"));
            })

             .AddApiExplorer(options =>
             {
                 options.GroupNameFormat = "'v'VVV"; // Format: v1, v2, etc.
                 options.SubstituteApiVersionInUrl = false;
             });

            services.AddHttpContextAccessor();
            ServiceConfigurationManager.ConfigureAuthentication(services, Configuration);

            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider()
               .GetRequiredService<IApiVersionDescriptionProvider>();

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo
                    {
                        Title = $"Pressford.News API {description.ApiVersion}",
                        Version = description.ApiVersion.ToString(),
                        Description = "Through this API you can Publish, Read News Articles and many more.",
                        Contact = new OpenApiContact()
                        {
                            Email = "shantanusrivastv@gmail.com",
                            Name = "Kumar Shantanu",
                            Url = new Uri("http://uk.linkedin.com/in/shaan")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });
                }

                var xmlCommentsFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = System.IO.Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                c.IncludeXmlComments(xmlCommentsFullPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.</br> " +
                                  "Enter 'Bearer' [space] and then your token in the text input below. </br> " +
                                  "Example: <b>'Bearer 12345abcdef</b>'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                        },
                         new List<string>()
                    }
                });
            });

            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                      .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();

                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes
                        .Add("application/vnd.kumar.hateoas+json");
                }
            });


            ServiceConfigurationManager.ConfigurePersistence(services, Configuration);
            ServiceConfigurationManager.ConfigureServiceLifeTime(services);
            services.AddResponseCaching();
            services.AddHttpCacheHeaders(
                    (expirationModelOptions) =>
                    {
                        expirationModelOptions.MaxAge = 60;
                        expirationModelOptions.CacheLocation =
                            Marvin.Cache.Headers.CacheLocation.Public;
                    },
                    (validationModelOptions) =>
                    {
                        validationModelOptions.MustRevalidate = true;
                    }
                    );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseExceptionHandler(app =>
            {
                app.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var errorResponse = new
                    {
                        error = "Something unexpected happened, please try again"
                    };
                    var json = JsonSerializer.Serialize(errorResponse);

                    await context.Response.WriteAsync(json, default);
                });
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                                       description.GroupName.ToUpperInvariant());
                }

                //c.SwaggerEndpoint("/swagger/v1/swagger.json", "News API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseCors("default");
            //app.UseResponseCaching(); //todo Its not working tried same in new apps it works something might be screwing this
            app.UseHttpCacheHeaders();//Author library
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    //TODO: Add this globally and check if Swagger shows it
    public class ProducesAttributeConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            controller.Filters.Add(new ProducesAttribute("application/json", "application/xml"));
        }
    }
}