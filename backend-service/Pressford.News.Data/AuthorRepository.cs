using Microsoft.EntityFrameworkCore;
using Pressford.News.Entities;
using System;
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
	}
}
