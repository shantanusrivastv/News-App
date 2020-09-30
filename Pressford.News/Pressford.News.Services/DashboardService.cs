using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pressford.News.Data;
using Pressford.News.Model;
using entity = Pressford.News.Entities;

namespace Pressford.News.Services
{
    public class DashboardService : IDashboardService
    {
        private IRepository<entity.Article> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardService(IRepository<entity.Article> repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public ICollection<Article> GetPublishedArticles()
        {
            var userName = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? null;
            var allArticles = _repository.FindBy(x => x.Author == userName) as ICollection<Article>;

            throw new NotImplementedException();
        }
    }
}