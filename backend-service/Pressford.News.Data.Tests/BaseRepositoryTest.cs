﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Pressford.News.Entities;

using FluentAssertions;

namespace Pressford.News.Data.Tests
{
	[TestFixture]
	public class BaseRepositoryTest
	{
		private Repository<Article> _sut;

		[SetUp]
		public void Setup()
		{
			_sut = new Repository<Article>(GetContext());
		}

		[Test]
		public void AuthorRepository_Searches_By_Author()
		{
			var result = _sut.FindBy(x => x.Author == "Mr Pressford").Single();

			result.Should().NotBeNull();
			result.Id.Should().Be(1);
		}

		private static PressfordNewsContext GetContext()
		{
			var options = new DbContextOptionsBuilder<PressfordNewsContext>()
							 .UseInMemoryDatabase(Guid.NewGuid().ToString())
							 .Options;

			var context = new PressfordNewsContext(options);
			context.Article.Add(new Article()
			{
				Author = "Mr Pressford",
				Id = 1,
				Body = "Sample New",
				Title = "In Memory testing"
			});

			context.Article.Add(new Article()
			{
				Author = "Dummy Author",
				Id = 2,
				Body = "Dummy Body",
				Title = "Should not be found in search result"
			});

			context.SaveChanges();
			return context;
		}
	}
}