using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Pressford.News.API.Controllers;
using Pressford.News.Model;
using Pressford.News.Services.Interfaces;

namespace Pressford.News.API.Tests.Controllers
{
	[TestFixture]
	public class ArticleControllerTests
	{
		private ArticleController _sut;
		private Mock<IArticleServices> _articleServices;

		[SetUp]
		public void Setup()
		{
			_articleServices = new Mock<IArticleServices>();
			_sut = new ArticleController(_articleServices.Object);
		}

		[Test]
		public async Task Should_Return_All_Articles()
		{
			// Arrange
			_articleServices.Setup(x => x.GetAllArticles()).ReturnsAsync(MockArticleResults());

			//Act
			var result = await _sut.GetAllArticles() as OkObjectResult;

			//Assert
			_articleServices.Verify(x => x.GetAllArticles(), Times.Once);
			result.Should().NotBeNull();
			result.Value.Should().BeOfType<List<ReadArticle>>().Which.Count.Should().Be(2);
			result.Should().BeOfType<OkObjectResult>().Which.StatusCode.Should().Be(200);
		}

		[Test]
		public async Task Should_Return_Single_Articles()
		{
			// Arrange
			_articleServices.Setup(x => x.GetSingleArticle(It.IsAny<int>()))
							.ReturnsAsync(MockArticleResults().First());

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
			_articleServices.Setup(x => x.CreateArticle(It.IsAny<ReadArticle>()))
							.ReturnsAsync(MockArticleResults().First());

			//Act
			var result = await _sut.CreateNewArticle(MockArticleResults().First()) as ObjectResult;

			//Assert
			_articleServices.Verify(x => x.CreateArticle(It.IsAny<ReadArticle>()), Times.Once);
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

		private static List<ReadArticle> MockArticleResults()
		{
			return
			[
			   new ReadArticle() { ArticleId =1, Author = "Author1"},
			   new ReadArticle() { ArticleId =2, Author = "Author2"}
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