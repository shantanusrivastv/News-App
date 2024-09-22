using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pressford.News.Services.Dependencies;

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
			services.AddControllers();
			services.AddHttpContextAccessor();
			ServiceConfigurationManager.ConfigureAuthentication(services, Configuration);

            services.AddSwaggerGen(c =>
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "Publish News ",
					Version = "1",
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

				//todo Enable Xml document and get documents from them to Swagger
				//var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				//var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

				//setupAction.IncludeXmlComments(xmlCommentsFullPath);
			});
			ServiceConfigurationManager.ConfigurePersistence(services, Configuration);
			ServiceConfigurationManager.ConfigureServiceLifeTime(services);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "News API v1");
				c.RoutePrefix = string.Empty;
			});

			app.UseCors("default");
			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}