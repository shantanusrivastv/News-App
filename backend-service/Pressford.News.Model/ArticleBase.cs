using System;
using System.ComponentModel.DataAnnotations;

namespace Pressford.News.Model
{
	public class ArticleBase
	{
		[Required]
		[MaxLength(200)]
		public string Title { get; set; }

		public string Body { get; set; }
	}

	public class ReadArticle : ArticleBase
	{
		public int Id { get; set; }
		public string Author { get; set; }
		public DateTime DatePublished { get; set; }
		public DateTime DateModified { get; set; }
	}

	public class UpdateArticle : ArticleBase
	{
		public int Id { get; set; }
	}
}