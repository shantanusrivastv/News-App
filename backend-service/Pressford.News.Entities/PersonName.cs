using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Entities
{
	public class PersonName
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string FullName => $"{FirstName} {LastName}";
		public string ReverseName => $"{LastName}, {FirstName}";
	}
}
