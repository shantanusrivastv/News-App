using System;

namespace Pressford.News.Entities
{
	public interface IEntityDate
	{
		public DateTime DatePublished { get; set; }

		public DateTime DateModified { get; set; }
	}
}