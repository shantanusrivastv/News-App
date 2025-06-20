using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Moq;
using NUnit.Framework;
using Pressford.News.API.Controllers;
using Pressford.News.Model;
using Pressford.News.Model.Helpers;
using Pressford.News.Model.ResourceParameters;
using Pressford.News.Services.Interfaces;
using Pressford.News.Services.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pressford.News.API.Tests.Controllers
{
	[TestFixture]
	public class ArticleControllerTests
	{
		private ArticleController _sut;
		private Mock<IArticleServices> _articleServices;
        private Mock<IDataShaper<ReadArticle>> _mockDataShaper;
		private Mock<ProblemDetailsFactory> _problemDetailsFactory;

        [SetUp]
		public void Setup()
		{
			_articleServices = new Mock<IArticleServices>();
			_mockDataShaper = new Mock<IDataShaper<ReadArticle>>();
			_problemDetailsFactory = new Mock<ProblemDetailsFactory>();
            _sut = new ArticleController(_articleServices.Object, _mockDataShaper.Object, _problemDetailsFactory.Object);

            // Mock HttpContext with header
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers["Accept"] = "application/json";
            _sut.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContext
            };
        }

		[Test]
		public async Task Should_Return_All_Articles()
		{
			// Arrange 
			_articleServices.Setup(x => x.GetAllArticles(It.IsAny<ArticleResourceParameters>())).ReturnsAsync(MockArticleResults());
			_articleServices.Setup(x => x.ValidateSortFieldsForArticle(It.IsAny<string>())).Returns(new List<string>());
            _articleServices.Setup(x => x.ValidateProjectionFieldsForArticle(It.IsAny<string>())).Returns(new List<string>());

            var input = new ArticleResourceParameters
			{
				PageNumber = 1,
				FilterQuery = "w.Pressford@pressford.com",
				PageSize = 20,
				SearchQuery = "Expresso",
				OrderBy = "title"
			};

            //Act
            var result = await _sut.GetAllArticles(input) as OkObjectResult;

			//Assert
			_articleServices.Verify(x => x.GetAllArticles(input), Times.Once);
			result.Should().NotBeNull();
			result.Value.Should().BeOfType<PagedList<ReadArticle>>().Which.Count.Should().Be(2);
			result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
		}

		[Test]
		public async Task Should_Return_Single_Articles()
		{
			// Arrange
			_articleServices.Setup(x => x.GetSingleArticle(It.IsAny<int>()))
							.ReturnsAsync(MockArticleResults().First());

            _articleServices.Setup(x => x.ValidateProjectionFieldsForArticle(It.IsAny<string>()))
                            .Returns(new List<string>());

            //Act
            var result = await _sut.GetSingleArticle(2) as ObjectResult;

			//Assert
			_articleServices.Verify(x => x.GetSingleArticle(It.IsAny<int>()), Times.Once);
			result.Should().NotBeNull();
			result.Value.Should().BeOfType<ReadArticle>();
			result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
		}

		[Test]
		public async Task Should_Return_NotFound_For_Non_Existing_Articles()
		{
			// Arrange
			_articleServices.Setup(x => x.GetSingleArticle(It.IsAny<int>()))
							.ReturnsAsync((ReadArticle)null);

			_articleServices.Setup(x => x.ValidateProjectionFieldsForArticle(It.IsAny<string>()))
							.Returns(new List<string>());

            //Act
            var result = await _sut.GetSingleArticle(222) as NotFoundResult;

			//Assert
			_articleServices.Verify(x => x.GetSingleArticle(It.IsAny<int>()), Times.Once);
			result.Should().NotBeNull();
			result.Should().NotBeOfType<ReadArticle>();
			result.Should().BeOfType<NotFoundResult>().Which.StatusCode.Should().Be(404);
		}

		[Test]
		public async Task Should_Create_New_Article()
		{
			// Arrange
			_articleServices.Setup(x => x.CreateArticle(It.IsAny<CreateArticle>()))
							.ReturnsAsync(MockArticleResults().First());

			//Act
			var result = await _sut.CreateNewArticle(MockCreateArticle().First()) as ObjectResult;

			//Assert
			_articleServices.Verify(x => x.CreateArticle(It.IsAny<CreateArticle>()), Times.Once);
			result.Should().NotBeNull();
			result.Value.Should().BeOfType<ReadArticle>();
			//Ideally the status code should be 201 , some implementation code is commented for reference
			result.Should().BeOfType<CreatedAtRouteResult>().Which.StatusCode.Should().Be(201);
		}

		[Test]
		public async Task Should_Update_Existing_Article()
		{
			// Arrange
			var updatedArticle = MockArticleResults().First();
			updatedArticle.Title = "New Title";
			_articleServices.Setup(x => x.UpdateArticle(It.IsAny<UpdateArticle>()))
							.ReturnsAsync(updatedArticle);

			//Act
			var result = await _sut.UpdateArticle(MockUpdateArticleResults().First()) as ObjectResult;

			//Assert
			_articleServices.Verify(x => x.UpdateArticle(It.IsAny<UpdateArticle>()), Times.Once);
			result.Should().NotBeNull();
			//Ideally the status code should be 201 , some implementation code is commented for reference
			result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
			result.Value.Should().BeOfType<ReadArticle>().Which.Title.Should().Be("New Title");
		}

		[Test]
		public async Task Should_Not_Update_For_UnAuthorised_Request()
		{
			// Arrange
			_articleServices.Setup(x => x.UpdateArticle(It.IsAny<UpdateArticle>()))
							.ReturnsAsync((ReadArticle)null);

			//Act
			var result = await _sut.UpdateArticle(MockUpdateArticleResults().First()) as UnauthorizedObjectResult;

			//Assert
			_articleServices.Verify(x => x.UpdateArticle(It.IsAny<UpdateArticle>()), Times.Once);
			result.Should().NotBeNull();
			//Ideally the status code should be 201 , some implementation code is commented for reference
			result.Should().BeOfType<UnauthorizedObjectResult>().Which.StatusCode.Should().Be(401);
		}

		[Test]
		public async Task Should_Accept_Valid_Delete_Request()
		{
			// Arrange
			_articleServices.Setup(x => x.RemoveArticle(It.IsAny<int>()))
							.ReturnsAsync(true);

			//Act
			var result = await _sut.DeleteArticle(2) as AcceptedResult;

			//Assert
			_articleServices.Verify(x => x.RemoveArticle(It.IsAny<int>()), Times.Once);
			result.Should().NotBeNull();
			result.Should().BeOfType<AcceptedResult>().Which.StatusCode.Should().Be(202);
		}

		[Test]
		public async Task Should_Reject_InValid_Delete_Request()
		{
			// Arrange
			_articleServices.Setup(x => x.RemoveArticle(It.IsAny<int>()))
							.ReturnsAsync(false);

			//Act
			var result = await _sut.DeleteArticle(2) as UnauthorizedObjectResult;

			//Assert
			_articleServices.Verify(x => x.RemoveArticle(It.IsAny<int>()), Times.Once);
			result.Should().NotBeNull();
			result.Should().BeOfType<UnauthorizedObjectResult>().Which.StatusCode.Should().Be(401);
		}

		private static PagedList<ReadArticle> MockArticleResults()
		{
			var articles = new List<ReadArticle>
			{
			   new ReadArticle() { ArticleId = 1,  Title = "Article 1 Title", Body = "Article 1 Body"},
			   new ReadArticle() { ArticleId = 2,   Title = "Article 2 Title", Body = "Article 2 Body"}
			};

			return new PagedList<ReadArticle>(articles, 2, 1, 10);
		}

		private static List<CreateArticle> MockCreateArticle()
		{
			return
			[
			   new CreateArticle() {Title = "Article 1 Title", Body = "Article 1 Body"},
			   new CreateArticle() {Title = "Article 2 Title", Body = "Article 2 Body"}
			];
		}

		private static List<UpdateArticle> MockUpdateArticleResults()
		{
			return
			[
			   new UpdateArticle() { ArticleId =1, Title = "Author1"},
			   new UpdateArticle() { ArticleId =2, Title = "Author2"}
			];
		}
	}
}