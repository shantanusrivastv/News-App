using System.ComponentModel.DataAnnotations;

namespace Pressford.News.Model
{
    public class Credentials
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}