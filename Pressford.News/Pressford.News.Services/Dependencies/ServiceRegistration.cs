using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pressford.News.Data;
using Pressford.News.Services.Mapper;

namespace Pressford.News.Services.Dependencies
{
    public class ServiceRegistration
    {
        public static void ConfigurePersistence(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<PressfordNewsContext>(options =>
                        options.UseSqlServer(config.GetConnectionString("PressfordNewsContext")));
        }

        public static void ConfigureLifeCycle(IServiceCollection services, IConfiguration config)
        {
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IArticleServices, ArticleServices>();
            services.AddAutoMapper(typeof(ArticleMapper));
        }
    }
}