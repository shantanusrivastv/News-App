using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Pressford.News.Entities;

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

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Id, 1);
        }

        private PressfordNewsContext GetContext()
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
                Title = "In Memeory testing"
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