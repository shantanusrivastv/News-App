namespace Pressford.News.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }

		//public PersonName Name { get; set; }
		public virtual UserLogin LoginInfo { get; set; }
	}
}