using System.Text;
using AutoMapper;
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
			services.AddDbContext<PressfordNewsContext>(options =>
						options.UseSqlServer(config.GetConnectionString("PressfordNewsContext")));
		}

		public static void ConfigureServiceLifeTime(IServiceCollection services)
		{
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
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