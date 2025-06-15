using Pressford.News.Entities;
using Pressford.News.Model;
using Pressford.News.Services.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Services
{
    public class ArticlePropertyMapping : PropertyMapping<ReadArticle,Article>
    {
        public ArticlePropertyMapping() : base(
        new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
        {
           { "age", new PropertyMappingValue(new[] { "DatePublished" }, revert: true) },
           { "titlewithbody", new PropertyMappingValue(new[] { "Title", "Body" }) },
           { "title", new PropertyMappingValue(new[] { "title" })}
        })
        { 


        }
    }
}
