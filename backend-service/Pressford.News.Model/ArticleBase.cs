using System;
using System.ComponentModel.DataAnnotations;

namespace Pressford.News.Model
{
	public class ArticleBase
	{
		[Required(ErrorMessage = "Article Name is required.")]
		[StringLength(20, MinimumLength = 5, ErrorMessage = "Article Name must be between 5 and 10 characters long.")]
		//[RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Name can only contain alphabetic characters.")]
		public string Title { get; set; }


		[Required(ErrorMessage = "Article Body is required.")]
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