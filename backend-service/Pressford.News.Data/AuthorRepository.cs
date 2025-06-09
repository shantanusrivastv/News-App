using Microsoft.EntityFrameworkCore;
using Pressford.News.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Data
{
	public class UserRepository(PressfordNewsContext context) : Repository<User>(context), IUserRepository
	{
		public async Task<List<User>> GetPublishedArticleWithinRange(DateTime startDtm, DateTime endDtm)
		{
			return await _context.User.FromSql($"AuthorsArticlesInYearRange {startDtm},{endDtm} ").ToListAsync();
		}

		public async Task<List<AuthorWithArticles>> GetAuthorView()
		{
			return await _context.AuthorWithArticles.ToListAsync();
		}

		//Refactor to its own repo
		public async Task<List<Article>> GetArticleCollection(IEnumerable<int> articleIds)
		{
			return await _context.Article.Where(x=> articleIds.Contains(x.Id))
                            .OrderBy(a => a.Author)
							.OrderBy(a => a.DatePublished)
							.ToListAsync();
        }
	}

}
