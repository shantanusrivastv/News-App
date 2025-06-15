using Pressford.News.Entities;
using Pressford.News.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Services.Mapper
{
    public class ArticlePropertyMapping : PropertyMapping<ReadArticle,Article>
    {
        public ArticlePropertyMapping() : base(
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
           { "age", new PropertyMappingValue(new[] { "DatePublished" }, reverseDirection: true) },
           { "titlewithbody", new PropertyMappingValue(new[] { "Title", "Body" }) },
           { "title", new PropertyMappingValue(new[] { "title" })}
        })
        { 


        }
    }
}
