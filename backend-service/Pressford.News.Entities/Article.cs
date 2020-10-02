using System;

namespace Pressford.News.Entities
{
    public class Article : IEntityDate
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Author { get; set; }

        public  DateTime DatePublished { get; set; }

        public  DateTime DateModified { get; set; }
    }
}