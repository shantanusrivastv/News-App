using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pressford.News.Model.ResourceParameters
{
    public class ArticleResourceParameters
    {
        private const int maxPagesize = 50;
        private int _pageSize = 20;
        private int _pageNumber = 1;
        public string? FilterQuery { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 1 : Math.Min(value, maxPagesize);
        }

        public string OrderBy { get; set; } = "title"; //Default Sorting
        public string Fields { get; set; } = String.Empty;
    }
}
