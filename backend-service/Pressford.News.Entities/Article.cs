using System;

namespace Pressford.News.Entities
{
	//We need One to many relationship between Author and Articles
	//Author is basically user with extra information like no of articles published
	public class Article : IEntityDate
	{
		public int Id { get; set; }

		public string Title { get; set; }

		public string Body { get; set; }

		public string Author { get; set; }

		//TOdo Convert to DateOnly we dont need time
		public DateTime DatePublished { get; set; }

		public DateTime DateModified { get; set; }
	}
}