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

namespace Pressford.News.Services.Services
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

		public async Task<List<model.ReadArticle>> GetAllPublishedArticle()
		{
			var allArticles = await _repository.GetAllAsync();
			return _mapper.Map<List<model.ReadArticle>>(allArticles);
		}

		public async Task<List<model.ReadArticle>> GetPublishedArticles()
		{
			var userName = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var allArticles = await _repository.FindByAsync(x => x.Author == userName);

			return _mapper.Map<List<model.ReadArticle>>(allArticles);
		}
	}
}