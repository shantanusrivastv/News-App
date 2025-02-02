﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Pressford.News.Data;
using Pressford.News.Entities;
using Pressford.News.Services.Mapper;

#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
using entity = Pressford.News.Entities;
using model = Pressford.News.Model;
#pragma warning restore CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.

namespace Pressford.News.Services.Tests
{
	[TestFixture]
	public class ArticleServicesTests
	{
		private Mock<IRepository<Article>> _repository;
		private Mock<IUserRepository> _usrRepository;
		private IMapper _mapper;
		private Mock<IHttpContextAccessor> _httpContextAccessor;
		private ArticleServices _sut;

		[SetUp]
		public void Setup()
		{
			_repository = new Mock<IRepository<Article>>();
			_usrRepository = new Mock<IUserRepository>();
			_httpContextAccessor = new Mock<IHttpContextAccessor>();

			var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new PressfordMapper()));
			//Seeing Up AutoMapper Profile to easy setup and we don't have to mock all mappings
			_mapper = new AutoMapper.Mapper(configuration);
			var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, "Username");
			_httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
							.Returns(nameIdentifierClaim)
							.Verifiable();
			_sut = new ArticleServices(_repository.Object, _usrRepository.Object, _mapper, _httpContextAccessor.Object);
		}

		[Test]
		public async Task Should_Create_New_Article()
		{
			// Arrange

			_repository.Setup(x => x.AddAsync(It.IsAny<entity.Article>()))
						.ReturnsAsync(MockEntityArticleModels().First())
						.Verifiable();

			//Act
			var result = await _sut.CreateArticle(MockArticleModels().First());

			//Assert
			_httpContextAccessor.Verify();
			_repository.Verify(x => x.AddAsync(It.IsAny<entity.Article>()), Times.Once);
			result.Should().NotBeNull();
			result.Author.Should().Be("Author1");
		}

		private static IEnumerable<model.ReadArticle> MockArticleModels()
		{
			return
			[
			   new model.ReadArticle() { ArticleId =1, Author = "Author1"},
			   new model.ReadArticle() { ArticleId =2, Author = "Author2"}
			];
		}

		private static IEnumerable<Article> MockEntityArticleModels()
		{
			return
			[
			  new entity.Article() { Id =1, Author = "Author1"},
			  new entity.Article() { Id =2, Author = "Author2"},
			];
		}
	}
}