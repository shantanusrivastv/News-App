using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NUnit.Framework;
using Pressford.News.API;
using Pressford.News.Model;

namespace Pressford.News.Functional.Tests
{
	[TestFixture]
	public class BasicTests
	{
		private HttpClient _client;
		private APIWebApplicationFactory _factory;

		[OneTimeSetUp]
		public void Init()
		{
			_factory = new APIWebApplicationFactory();
			_client = _factory.CreateClient();
		}

		[Test]
		public async Task Should_Return_All_Articles()
		{
			// Act
			var response = await _client.GetAsync("api/Article");
			var stringResponse = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<ArticleBase[]>(stringResponse);

			// Assert
			result.Should().NotBeNull();
			response.EnsureSuccessStatusCode();
		}

		[Test]
		public async Task Should_Authenticate()
		{
			// Arrange
			var client = _factory.WithWebHostBuilder(builder =>
			{
				builder.ConfigureTestServices(services =>
				{
					services.AddAuthentication("Test")
							.AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
				});
			})
				.CreateClient(new WebApplicationFactoryClientOptions
				{
					AllowAutoRedirect = false,
				});

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");

			var article = new ArticleBase()
			{
				Title = "Test Title",
				Body = "Test Body"
			};

			//var stringContent = new FormUrlEncodedContent(new[]
			//			{
			//				new KeyValuePair<string, string>("Title", "Test Title"),
			//				new KeyValuePair<string, string>("Body", "Test Body"),
			//			});

			var payload = new Dictionary<string, string>
			{
			  {"Title", "Test Title"},
			  {"Body", "Test Body"}
			};

			string strPayload = JsonConvert.SerializeObject(payload);
			HttpContent content = new StringContent(strPayload, Encoding.UTF8, "application/json");

			// Act
			var response = await client.GetAsync("api/Article");
			var stringResponse = await response.Content.ReadAsStringAsync();
			var result = JsonConvert.DeserializeObject<ArticleBase[]>(stringResponse);

			// Assert
			result.Should().NotBeNull();
			response.EnsureSuccessStatusCode();
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			_client.Dispose();
			_factory.Dispose();
		}
	}

	public class APIWebApplicationFactory : WebApplicationFactory<Startup>
	{
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "DEVELOPMENT");
			builder.UseEnvironment("DEVELOPMENT");
		}
	}
}