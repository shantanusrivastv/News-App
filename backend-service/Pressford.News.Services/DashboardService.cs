using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Pressford.News.Data;
using Pressford.News.Services.Interfaces;
using entity = Pressford.News.Entities;
using model = Pressford.News.Model;

namespace Pressford.News.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IRepository<entity.Article> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public DashboardService(IRepository<entity.Article> repository,
                                IHttpContextAccessor httpContextAccessor,
                                IMapper mapper)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public Task<List<model.ReadArticle>> GetAllPublishedArticle()
        {
            var allArticles = _repository.GetAll();
            return Task.FromResult(_mapper.Map<List<model.ReadArticle>>(allArticles));
        }

        public Task<List<model.ReadArticle>> GetPublishedArticles()
        {
            var userName = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var allArticles = _repository.FindBy(x => x.Author == userName).ToList();

            return Task.FromResult(_mapper.Map<List<model.ReadArticle>>(allArticles));
        }
    }
}