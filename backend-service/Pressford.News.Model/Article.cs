using System;

namespace Pressford.News.Model
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Author { get; set; }

        public DateTime DatePublished { get; set; }
    }
}