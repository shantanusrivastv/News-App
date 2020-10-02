using System.ComponentModel.DataAnnotations.Schema;

namespace Pressford.News.Entities
{
    public class UserLogin
    {
        public string Username { get; set; }

        public string Password { get; set; }
        public RoleType Role { get; set; }

        [NotMapped]
        public string Token { get; set; }

        public virtual User User { get; set; }
    }
}