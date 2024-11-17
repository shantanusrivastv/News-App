using Pressford.News.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pressford.News.Data
{
	public interface IUserRepository
	{
		Task<List<AuthorWithArticles>> GetAuthorView();
		Task<List<User>> GetPublishedArticleWithinRange(DateTime startDtm, DateTime endDtm);
	}
}