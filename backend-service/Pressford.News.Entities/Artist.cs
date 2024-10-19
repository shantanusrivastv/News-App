using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Entities
{
	public class Artist
	{
		public int ArtistId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public List<Cover> Covers { get; set; } = new();
	}
}
