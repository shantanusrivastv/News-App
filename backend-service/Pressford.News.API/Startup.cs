using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pressford.News.Services.Dependencies;
using System;
using System.Collections.Generic;

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
				//setupAction.Conventions.Add(new ProducesAttributeConvention());

			}).AddNewtonsoftJson()
			  .AddXmlDataContractSerializerFormatters();

			services.AddHttpContextAccessor();
			ServiceConfigurationManager.ConfigureAuthentication(services, Configuration);

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
	//TODO: Add this globally and check if Swagger shows it
	public class ProducesAttributeConvention : IControllerModelConvention
	{
		public void Apply(ControllerModel controller)
		{
			controller.Filters.Add(new ProducesAttribute("application/json", "application/xml"));
		}
	}
}