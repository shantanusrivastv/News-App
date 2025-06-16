using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Pressford.News.Data;
using Pressford.News.Entities;
using Pressford.News.Model;
using Pressford.News.Services.Interfaces;
using Pressford.News.Services.Mapper;
using Pressford.News.Services.Services;
using System.Text;

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
									//Moved the logic to appsettings.Development.json
									//options.LogTo(System.Console.WriteLine, 
									//		new[]
									//		{
									//			DbLoggerCategory.Database.Command.Name,
									//			DbLoggerCategory.ChangeTracking.Name
									//		}, Microsoft.Extensions.Logging.LogLevel.Information);
								}); 
			}
			else
			{
				services.AddDbContext<PressfordNewsContext>(options =>
				{
					options.EnableSensitiveDataLogging();//Disable in production
					options.UseSqlServer(config.GetConnectionString("SqlLocalDb"), b => b.MigrationsAssembly("Pressford.News.Data"));
					//options.UseSqlite(config.GetConnectionString("DefaultConnection"));
				});
			}
		}

		public static void ConfigureServiceLifeTime(IServiceCollection services)
		{
			//We want to share the same DbContext instance throughout a single HTTP request.
			services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
			services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            //The service layer is stateless hence transient
            //services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<IPropertyMappingService, PropertyMappingService>(); //It makes sense for singleton as even automapper is the same.
            services.AddSingleton<IDataShaper<ReadArticle>, DataShaper<ReadArticle>>();
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