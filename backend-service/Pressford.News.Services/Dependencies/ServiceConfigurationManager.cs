using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pressford.News.Data;
using Pressford.News.Services.Interfaces;
using Pressford.News.Services.Mapper;

namespace Pressford.News.Services.Dependencies
{
	public static class ServiceConfigurationManager
	{
		public static void ConfigurePersistence(IServiceCollection services, IConfiguration config)
		{
			
			string envt = config.GetValue<string>("ASPNETCORE_ENVIRONMENT");

			if (envt.Trim().ToUpper() == "DEVELOPMENT")
			{
				services.AddDbContext<PressfordNewsContext>(options =>
								{
									options.EnableSensitiveDataLogging();//Disable in production
									options.UseSqlServer(config.GetConnectionString("PressfordNewsContext"));
									options.LogTo(System.Console.WriteLine, 
											new[]
											{
												DbLoggerCategory.Database.Command.Name,
												DbLoggerCategory.ChangeTracking.Name
											}, Microsoft.Extensions.Logging.LogLevel.Information);
								}); 
			}
			else
			{
				services.AddDbContext<PressfordNewsContext>(options =>
				{

					services.AddDbContext<PressfordNewsContext>(options =>
					{
						options.EnableSensitiveDataLogging();//Disable in production
						options.UseSqlServer(config.GetConnectionString("PressfordNewsContext"));
						options.LogTo(System.Console.WriteLine,
								new[]
								{
												DbLoggerCategory.Database.Command.Name,
												DbLoggerCategory.ChangeTracking.Name
								}, Microsoft.Extensions.Logging.LogLevel.Information);
					});
					
					//options.UseSqlite(config.GetConnectionString("DefaultConnection"));
				});
			}
		}

		public static void ConfigureServiceLifeTime(IServiceCollection services)
		{
			//We want to share the same DbContext instance throughout a single HTTP request.
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			//The service layer is stateless hence transient
			services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
			services.AddTransient<IArticleServices, ArticleServices>();
			services.AddTransient<IUserService, UserService>();
			services.AddTransient<IArticleLikeService, ArticleLikeService>();
			services.AddTransient<IDashboardService, DashboardService>();
			services.AddAutoMapper(typeof(PressfordMapper));
		}

		public static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
		{
			var appSecret = configuration.GetValue<string>("AppSettings:Secret");
			var key = Encoding.ASCII.GetBytes(appSecret);
			services.AddAuthentication(x =>
			{
				x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(x =>
			{
				x.RequireHttpsMetadata = false;
				x.SaveToken = true;
				x.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false
				};
			});
		}
	}
}