﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Pressford.News.Data;
using Pressford.News.Entities;
using Pressford.News.Services.Mapper;

using entity = Pressford.News.Entities;

using model = Pressford.News.Model;

namespace Pressford.News.Services.Tests
{
    [TestFixture]
    public class ArticleServicesTests
    {
        private Mock<IRepository<Article>> _repository;
        private IMapper _mapper;
        private Mock<IHttpContextAccessor> _httpContextAccessor;
        private ArticleServices _sut;

        [SetUp]
        public void Setup()
        {
            _repository = new Mock<IRepository<Article>>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new PressfordMapper()));
            //Seeing Up AutoMapper Profile to easy setup and we don't have to mock all mappings
            _mapper = new AutoMapper.Mapper(configuration);
            _sut = new ArticleServices(_repository.Object, _mapper, _httpContextAccessor.Object);
        }

        [Test]
        public async Task Should_Create_New_Article()
        {
            // Arrange
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, "Username");
            _httpContextAccessor.Setup(x => x.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier))
                                .Returns(nameIdentifierClaim)
                                .Verifiable();

            _repository.Setup(x => x.AddAsync(It.IsAny<entity.Article>()))
                        .ReturnsAsync(MockEntityArticleModels().First())
                        .Verifiable();

            //Act
            var result = await _sut.CreateArticle(MockArticleModels().First());

            //Assert
            _httpContextAccessor.Verify();
            _repository.Verify(x => x.AddAsync(It.IsAny<entity.Article>()), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Author, "Author1");
        }

        private static IEnumerable<model.Article> MockArticleModels()
        {
            return new List<model.Article>()
            {
               new model.Article() { Id =1, Author = "Author1"},
               new model.Article() { Id =2, Author = "Author2"}
            };
        }

        private static IEnumerable<Article> MockEntityArticleModels()
        {
            return new List<entity.Article>()
            {
              new entity.Article() { Id =1, Author = "Author1"},
              new entity.Article() { Id =2, Author = "Author2"},
            };
        }
    }
}