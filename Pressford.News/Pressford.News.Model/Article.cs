using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Model
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Author { get; set; }

        public virtual DateTime DatePublished { get; set; }
    }
}